using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using BaseCleanArchitecture.Application.Features.Usuarios.Commands.CreateUsuario;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BC = BCrypt.Net.BCrypt;

namespace BaseCleanArchitecture.Application.Features.Usuarios.Commands.CreateUsuario
{
    public class CreateUsuarioHandler : IRequestHandler<CreateUsuarioCommand, UsuarioResponseDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IPuestoRepository _puestoRepository;
        private readonly IRolRepository _rolRepository;
        private readonly IMapper _mapper;
        private readonly IUserContext _userContext;
        private readonly IMailService _mailService;

        public CreateUsuarioHandler(
            IUsuarioRepository usuarioRepository,
            IPuestoRepository puestoRepository,
            IRolRepository rolRepository,
            IMapper mapper,
            IUserContext userContext,
            IMailService mailService)
        {
            _usuarioRepository = usuarioRepository;
            _puestoRepository = puestoRepository;
            _rolRepository = rolRepository;
            _mapper = mapper;
            _userContext = userContext;
            _mailService = mailService;
        }

        public async Task<UsuarioResponseDto> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
        {
            // Obtener el userId del usuario autenticado
            var userId = _userContext.GetCurrentUserId();
            if (userId == null)
                throw new System.Exception("Usuario no autenticado");
            
            var usuarioCreador = await _usuarioRepository.GetByIdAsync(userId.Value);
            if (usuarioCreador == null)
                throw new System.Exception("Usuario creador no encontrado");
            
            // Validar que el puesto existe si se proporciona
            string puestoNombre = "Sin puesto";
            if (request.Usuario.IdPuesto != 0)
            {
                var puestos = await _puestoRepository.GetAllAsync();
                var puesto = puestos.FirstOrDefault(p => p.Id == request.Usuario.IdPuesto);
                if (puesto == null)
                    throw new System.Exception("El puesto seleccionado no existe");
                puestoNombre = puesto.Nombre;
            }
            
            // Validar que el rol existe si se proporciona
            string rolNombre = "Sin rol";
            if (request.Usuario.IdRol != 0)
            {
                var roles = await _rolRepository.GetAllAsync();
                var rol = roles.FirstOrDefault(r => r.Id == request.Usuario.IdRol);
                if (rol == null)
                    throw new System.Exception("El rol seleccionado no existe");
                rolNombre = rol.Nombre;
            }
            
            var usuario = _mapper.Map<Domain.Entities.Usuario>(request.Usuario);
            usuario.IdUser = userId.Value;
            usuario.IdEmpresa = usuarioCreador.IdEmpresa; // Heredar empresa del usuario autenticado
            usuario.Status = true;
            usuario.DateCreate = DateTime.UtcNow;
            
            // Hashear la contraseña antes de guardar
            var passwordToHash = request.Usuario.Contrasena ?? GenerateRandomPassword();
            usuario.Contrasena = BC.HashPassword(passwordToHash);

            var usuarioCreado = await _usuarioRepository.AddAsync(usuario);
            if (usuarioCreado == null)
                throw new System.Exception("Error al crear el usuario");
            
            // Enviar email de bienvenida
            try
            {
                await SendWelcomeEmailAsync(usuarioCreado, passwordToHash, puestoNombre, rolNombre);
            }
            catch (Exception ex)
            {
                // Log del error pero no fallar la creación del usuario
                // En un entorno de producción, usar un logger real
                Console.WriteLine($"Error al enviar email de bienvenida: {ex.Message}");
            }
            
            // Crear response
            var response = _mapper.Map<UsuarioResponseDto>(usuarioCreado);
                
            return response;
        }

        private string GenerateRandomPassword()
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789!@#$%^&*()_+";
            var random = new System.Random();
            var password = new System.Text.StringBuilder();

            for (int i = 0; i < 12; i++)
            {
                password.Append(chars[random.Next(chars.Length)]);
            }

            return password.ToString();
        }

        private async Task SendWelcomeEmailAsync(Usuario usuario, string password, string puestoNombre, string rolNombre)
        {
            var notification = new Notification
            {
                Template = "USUARIO_BIENVENIDA",
                Module = "USUARIOS",
                Recipients = new List<string> { usuario.Correo },
                Subject = "Bienvenido a IESPRO - Tus credenciales de acceso",
                Params = new Dictionary<string, string>
                {
                    { "nombre", usuario.Nombre },
                    { "apellido", usuario.Apellido ?? "" },
                    { "correo", usuario.Correo },
                    { "contrasena", password },
                    { "puesto", puestoNombre },
                    { "rol", rolNombre },
                    { "fechaCreacion", DateTime.UtcNow.ToString("dd/MM/yyyy HH:mm") }
                }
            };

            await _mailService.SendEmailNotificationAsync(notification);
        }
    }
} 