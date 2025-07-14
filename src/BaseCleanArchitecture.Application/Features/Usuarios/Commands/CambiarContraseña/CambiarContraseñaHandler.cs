using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Domain.Resources;
using System.Security.Cryptography;
using System.Text;
using BaseCleanArchitecture.Application.Features.Usuarios.Commands.CambiarContraseña;

namespace BaseCleanArchitecture.Application.Features.Usuarios.Commands.CambiarContraseña
{
    public class CambiarContraseñaHandler : IRequestHandler<CambiarContraseñaCommand, bool>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public CambiarContraseñaHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<bool> Handle(CambiarContraseñaCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
            if (usuario == null)
                throw new Exception("Usuario no encontrado");

            // Verificar contraseña actual
            var hashedPasswordActual = HashPassword(request.ContraseñaActual);
            if (usuario.Contrasena != hashedPasswordActual)
                throw new Exception("La contraseña actual es incorrecta");

            // Validar nueva contraseña
            if (string.IsNullOrEmpty(request.NuevaContraseña) || request.NuevaContraseña.Length < 8)
                throw new Exception(Messages.ContraseñaInvalida);

            // Actualizar contraseña
            usuario.Contrasena = HashPassword(request.NuevaContraseña);
            usuario.DateUpdate = DateTime.UtcNow;

            await _usuarioRepository.UpdateAsync(usuario);
            return true;
        }

        private string HashPassword(string password)
        {
            using (var sha256 = SHA256.Create())
            {
                var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
                return Convert.ToBase64String(hashedBytes);
            }
        }
    }
} 