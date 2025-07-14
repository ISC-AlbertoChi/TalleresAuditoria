using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using BaseCleanArchitecture.Application.Features.Usuarios.Commands.CreateUsuario;
using BaseCleanArchitecture.Application.Features.Usuarios.Commands.UpdateUsuario;
using BaseCleanArchitecture.Application.Features.Usuarios.Commands.DeleteUsuario;
using BaseCleanArchitecture.Application.Features.Usuarios.Commands.CambiarContraseña;
using BaseCleanArchitecture.Application.DTOs;
using System.Security.Claims;
using BaseCleanArchitecture.Domain.Interfaces;
using Microsoft.AspNetCore.Http;
using AutoMapper;

namespace BaseCleanArchitecture.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UsuariosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsuariosController(
            IMediator mediator,
            IUsuarioRepository usuarioRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Endpoint universal para obtener usuarios con diferentes opciones
        /// </summary>
        /// <param name="id">ID específico del usuario (opcional)</param>
        /// <param name="page">Número de página para paginación (opcional)</param>
        /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
        /// <param name="search">Término de búsqueda en nombre (opcional)</param>
        /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
        /// <param name="status">Filtrar por status (opcional)</param>
        /// <returns>Lista de usuarios o usuario específico</returns>
        [HttpGet]
        public async Task<ActionResult<ApiResponse<object>>> Get(
            [FromQuery] int? id = null,
            [FromQuery] int? page = null,
            [FromQuery] int pageSize = 10,
            [FromQuery] string? search = null,
            [FromQuery] bool combo = false,
            [FromQuery] bool? status = null)
        {
            try
            {
                // Si se especifica un ID, retornar ese usuario específico
                if (id.HasValue)
                {
                    var usuario = await _usuarioRepository.GetByIdAsync(id.Value);
                    if (usuario == null)
                        return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Usuario no encontrado" });

                    return Ok(new ApiResponse<object> { Success = true, Data = usuario });
                }

                // Si es para combo, retornar solo ID y Nombre
                if (combo)
                {
                    var usuariosCombo = await _usuarioRepository.GetAllAsync();
                    var comboData = usuariosCombo
                        .Where(u => !status.HasValue || u.Status == status.Value)
                        .Select(u => new { u.Id, Nombre = $"{u.Nombre} {u.Apellido}" })
                        .ToList();

                    return Ok(new ApiResponse<object> { Success = true, Data = comboData });
                }

                // Obtener todos los usuarios con filtros
                var query = await _usuarioRepository.GetAllAsync();
                var usuarios = query.AsQueryable();

                // Aplicar filtro de búsqueda
                if (!string.IsNullOrEmpty(search))
                {
                    usuarios = usuarios.Where(u => u.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase) || 
                                                  u.Apellido.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                // Aplicar filtro de status
                if (status.HasValue)
                {
                    usuarios = usuarios.Where(u => u.Status == status.Value);
                }

                // Aplicar paginación si se especifica
                if (page.HasValue)
                {
                    var totalCount = usuarios.Count();
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    var pagedData = usuarios
                        .Skip((page.Value - 1) * pageSize)
                        .Take(pageSize)
                        .ToList();

                    var result = new
                    {
                        Data = pagedData,
                        Pagination = new
                        {
                            CurrentPage = page.Value,
                            PageSize = pageSize,
                            TotalCount = totalCount,
                            TotalPages = totalPages
                        }
                    };

                    return Ok(new ApiResponse<object> { Success = true, Data = result });
                }

                // Retornar todos sin paginación
                var allData = usuarios.ToList();
                return Ok(new ApiResponse<object> { Success = true, Data = allData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Crear un nuevo usuario
        /// </summary>
        /// <param name="usuario">Datos del usuario a crear</param>
        /// <returns>Usuario creado</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateUsuarioDto usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Datos de entrada inválidos", 
                        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado", "Usuario no autenticado"));
                }

                var userId = int.Parse(userIdClaim.Value);
                var command = new CreateUsuarioCommand(usuario);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(Create), new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Usuario creado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear el usuario", ex.Message));
            }
        }

        /// <summary>
        /// Actualizar un usuario existente
        /// </summary>
        /// <param name="id">ID del usuario</param>
        /// <param name="usuario">Datos actualizados del usuario</param>
        /// <returns>Usuario actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateUsuarioDto usuario)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Datos de entrada inválidos", 
                        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado", "Usuario no autenticado"));
                }

                var userId = int.Parse(userIdClaim.Value);
                var command = new UpdateUsuarioCommand(id, usuario, userId);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Usuario actualizado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar el usuario", ex.Message));
            }
        }

        /// <summary>
        /// Eliminar un usuario
        /// </summary>
        /// <param name="id">ID del usuario a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteUsuarioCommand { Id = id };
                var result = await _mediator.Send(command);
                
                if (result == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Usuario no encontrado"));
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Usuario eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar el usuario", ex.Message));
            }
        }
    }
} 