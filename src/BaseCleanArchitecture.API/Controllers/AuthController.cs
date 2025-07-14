using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.IdentityModel.Tokens.Jwt;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Application.DTOs;
using BC = BCrypt.Net.BCrypt;

namespace BaseCleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IConfiguration _configuration;
        private readonly ILogger<AuthController> _logger;

        public AuthController(
            IUsuarioRepository usuarioRepository, 
            IRolRepository rolRepository,
            IConfiguration configuration,
            ILogger<AuthController> logger)
        {
            _usuarioRepository = usuarioRepository;
            _rolRepository = rolRepository;
            _configuration = configuration;
            _logger = logger;
        }

        /// <summary>
        /// Autenticación de usuario con validación de contraseña hasheada
        /// </summary>
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            try
            {
                // Validar datos de entrada
                if (string.IsNullOrEmpty(request.Correo) || string.IsNullOrEmpty(request.Contrasena))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Datos incompletos", 
                        new List<string> { "El correo y la contraseña son requeridos" }));
                }

                _logger.LogInformation($"Intento de login para el correo: {request.Correo}");

                // Buscar usuario por correo
                var usuario = await _usuarioRepository.GetByEmailAsync(request.Correo);
                if (usuario == null)
                {
                    _logger.LogWarning($"Usuario no encontrado: {request.Correo}");
                    return NotFound(ApiResponse<object>.ErrorResponse("Correo no registrado", 
                        new List<string> { "El correo electrónico no está registrado en el sistema" }));
                }

                // Verificar que el usuario esté activo
                if (!usuario.Status)
                {
                    _logger.LogWarning($"Intento de login con usuario inactivo: {request.Correo}");
                    return StatusCode(403, ApiResponse<object>.ErrorResponse("Usuario inactivo", 
                        new List<string> { "Este usuario está inactivo. Contacte al administrador" }));
                }

                // Verificar contraseña con BCrypt
                if (!BC.Verify(request.Contrasena, usuario.Contrasena))
                {
                    _logger.LogWarning($"Contraseña incorrecta para el usuario: {request.Correo}");
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Credenciales incorrectas", 
                        new List<string> { "La contraseña ingresada es incorrecta" }));
                }

                // Generar tokens
                var accessToken = await GenerateAccessToken(usuario);
                var refreshToken = GenerateRefreshToken(usuario);
                var tokenExpiration = DateTime.UtcNow.AddHours(4); // Access token expira en 4 horas

                _logger.LogInformation($"Login exitoso para el usuario: {request.Correo}");

                // Construir respuesta exitosa
                var loginResponse = new LoginResponse
                {
                    Token = accessToken,
                    RefreshToken = refreshToken,
                    TokenExpiration = tokenExpiration,
                    Usuario = new UsuarioInfo
                    {
                        Id = usuario.Id,
                        Nombre = usuario.Nombre,
                        Apellido = usuario.Apellido,
                        Correo = usuario.Correo,
                        IdEmpresa = usuario.IdEmpresa,
                        IdRol = usuario.IdRol,
                        Status = usuario.Status
                    }
                };

                return Ok(new ApiResponse<LoginResponse>
                {
                    Success = true,
                    Mensaje = "Login exitoso",
                    Data = loginResponse
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el login");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor", 
                    new List<string> { "Ocurrió un error inesperado. Intente nuevamente" }));
            }
        }

        /// <summary>
        /// Renovar access token usando refresh token
        /// </summary>
        [HttpPost("refresh")]
        public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RefreshToken))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Refresh token requerido", "Refresh token requerido"));
                }

                // Validar y decodificar el refresh token
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtKey = _configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

                try
                {
                    tokenHandler.ValidateToken(request.RefreshToken, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"] ?? "IESPRO-API",
                        ValidateAudience = true,
                        ValidAudience = _configuration["Jwt:Audience"] ?? "IESPRO-Client",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                    var tokenType = jwtToken.Claims.FirstOrDefault(x => x.Type == "TokenType")?.Value;

                    if (tokenType != "Refresh")
                    {
                        return BadRequest(ApiResponse<object>.ErrorResponse("Token inválido", "Token inválido"));
                    }

                    // Obtener usuario
                    var usuario = await _usuarioRepository.GetByIdAsync(userId);
                    if (usuario == null || !usuario.Status)
                    {
                        return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no válido", "Usuario no válido"));
                    }

                    // Obtener el nombre del rol si existe
                    string? rolNombre = null;
                    if (usuario.IdRol.HasValue)
                    {
                        var rol = await _rolRepository.GetByIdAsync(usuario.IdRol.Value);
                        rolNombre = rol?.Nombre;
                    }

                    // Generar nuevos tokens
                    var newAccessToken = await GenerateAccessToken(usuario);
                    var newRefreshToken = GenerateRefreshToken(usuario);
                    var tokenExpiration = DateTime.UtcNow.AddHours(4);

                    var refreshResponse = new LoginResponse
                    {
                        Token = newAccessToken,
                        RefreshToken = newRefreshToken,
                        TokenExpiration = tokenExpiration,
                        Usuario = new UsuarioInfo
                        {
                            Id = usuario.Id,
                            Nombre = usuario.Nombre,
                            Apellido = usuario.Apellido,
                            Correo = usuario.Correo,
                            IdEmpresa = usuario.IdEmpresa,
                            IdRol = usuario.IdRol,
                            Status = usuario.Status
                        }
                    };

                    return Ok(new ApiResponse<LoginResponse>
                    {
                        Success = true,
                        Mensaje = "Token renovado exitosamente",
                        Data = refreshResponse
                    });
                }
                catch (Exception)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Refresh token inválido o expirado", "Refresh token inválido o expirado"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante la renovación del token");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor", "Error interno del servidor"));
            }
        }

        /// <summary>
        /// Cerrar sesión e invalidar refresh token
        /// </summary>
        [HttpPost("logout")]
        [Authorize]
        public async Task<IActionResult> Logout([FromBody] LogoutRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.RefreshToken))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Refresh token requerido", "Refresh token requerido"));
                }

                // Aquí podrías implementar una blacklist de tokens
                // Por ahora solo validamos que el token sea válido
                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtKey = _configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

                try
                {
                    tokenHandler.ValidateToken(request.RefreshToken, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"] ?? "IESPRO-API",
                        ValidateAudience = true,
                        ValidAudience = _configuration["Jwt:Audience"] ?? "IESPRO-Client",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    // TODO: Agregar el token a una blacklist en la base de datos
                    // await _tokenBlacklistService.AddToBlacklistAsync(request.RefreshToken);

                    return Ok(new ApiResponse<object>
                    {
                        Success = true,
                        Mensaje = "Sesión cerrada exitosamente"
                    });
                }
                catch (Exception)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Refresh token inválido", "Refresh token inválido"));
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error durante el logout");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor", "Error interno del servidor"));
            }
        }

        /// <summary>
        /// Obtener información del usuario autenticado actual
        /// </summary>
        [HttpGet("me")]
        [Authorize]
        public async Task<IActionResult> GetProfile()
        {
            try
            {
                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado", "Usuario no autenticado"));
                }

                var userId = int.Parse(userIdClaim.Value);
                var usuario = await _usuarioRepository.GetByIdAsync(userId);
                
                if (usuario == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Usuario no encontrado", "Usuario no encontrado"));
                }

                var profileInfo = new UsuarioInfo
                {
                    Id = usuario.Id,
                    Nombre = usuario.Nombre,
                    Apellido = usuario.Apellido,
                    Correo = usuario.Correo,
                    IdEmpresa = usuario.IdEmpresa,
                    IdRol = usuario.IdRol,
                    Status = usuario.Status
                };

                return Ok(new ApiResponse<UsuarioInfo>
                {
                    Success = true,
                    Mensaje = "Perfil obtenido exitosamente",
                    Data = profileInfo
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al obtener perfil del usuario");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor", "Error interno del servidor"));
            }
        }

        /// <summary>
        /// Validar si un token es válido
        /// </summary>
        [HttpPost("validate")]
        public async Task<IActionResult> ValidateToken([FromBody] ValidateTokenRequest request)
        {
            try
            {
                if (string.IsNullOrEmpty(request.Token))
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Token requerido", "Token requerido"));
                }

                var tokenHandler = new JwtSecurityTokenHandler();
                var jwtKey = _configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters";
                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));

                try
                {
                    tokenHandler.ValidateToken(request.Token, new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = key,
                        ValidateIssuer = true,
                        ValidIssuer = _configuration["Jwt:Issuer"] ?? "IESPRO-API",
                        ValidateAudience = true,
                        ValidAudience = _configuration["Jwt:Audience"] ?? "IESPRO-Client",
                        ValidateLifetime = true,
                        ClockSkew = TimeSpan.Zero
                    }, out SecurityToken validatedToken);

                    var jwtToken = (JwtSecurityToken)validatedToken;
                    var userId = int.Parse(jwtToken.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value);
                    var tokenType = jwtToken.Claims.FirstOrDefault(x => x.Type == "TokenType")?.Value;

                    var validationResponse = new TokenValidationResponse
                    {
                        IsValid = true,
                        UserId = userId,
                        TokenType = tokenType ?? "Unknown",
                        ExpiresAt = jwtToken.ValidTo
                    };

                    return Ok(new ApiResponse<TokenValidationResponse>
                    {
                        Success = true,
                        Mensaje = "Token válido",
                        Data = validationResponse
                    });
                }
                catch (Exception)
                {
                    return Ok(new ApiResponse<TokenValidationResponse>
                    {
                        Success = true,
                        Mensaje = "Token inválido",
                        Data = new TokenValidationResponse
                        {
                            IsValid = false,
                            UserId = 0,
                            TokenType = "Invalid",
                            ExpiresAt = DateTime.MinValue
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al validar token");
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error interno del servidor", "Error interno del servidor"));
            }
        }

        private async Task<string> GenerateAccessToken(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim(ClaimTypes.Name, $"{usuario.Nombre} {usuario.Apellido}"),
                new Claim("IdEmpresa", usuario.IdEmpresa?.ToString() ?? "0"),
                new Claim("IdRol", usuario.IdRol?.ToString() ?? "0"),
                new Claim("TokenType", "Access")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "IESPRO-API",
                audience: _configuration["Jwt:Audience"] ?? "IESPRO-Client",
                claims: claims,
                expires: DateTime.UtcNow.AddHours(4), // 4 horas
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private string GenerateRefreshToken(Usuario usuario)
        {
            var jwtKey = _configuration["Jwt:Key"] ?? "your-super-secret-key-with-at-least-32-characters";
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var claims = new[]
            {
                new Claim(ClaimTypes.NameIdentifier, usuario.Id.ToString()),
                new Claim(ClaimTypes.Email, usuario.Correo),
                new Claim("TokenType", "Refresh")
            };

            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"] ?? "IESPRO-API",
                audience: _configuration["Jwt:Audience"] ?? "IESPRO-Client",
                claims: claims,
                expires: DateTime.UtcNow.AddDays(7), // 7 días
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }

    public class LoginRequest
    {
        public required string Correo { get; set; }
        public required string Contrasena { get; set; }
    }

    public class LoginResponse
    {
        public string Token { get; set; } = null!;
        public string RefreshToken { get; set; } = null!;
        public DateTime TokenExpiration { get; set; }
        public UsuarioInfo Usuario { get; set; } = null!;
    }

    public class UsuarioInfo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = null!;
        public string Apellido { get; set; } = null!;
        public string Correo { get; set; } = null!;
        public int? IdEmpresa { get; set; }
        public int? IdRol { get; set; }
        public bool Status { get; set; }
    }

    public class RefreshTokenRequest
    {
        public required string RefreshToken { get; set; }
    }

    public class LogoutRequest
    {
        public required string RefreshToken { get; set; }
    }

    public class ValidateTokenRequest
    {
        public required string Token { get; set; }
    }

    public class TokenValidationResponse
    {
        public bool IsValid { get; set; }
        public int UserId { get; set; }
        public string TokenType { get; set; } = null!;
        public DateTime ExpiresAt { get; set; }
    }
} 