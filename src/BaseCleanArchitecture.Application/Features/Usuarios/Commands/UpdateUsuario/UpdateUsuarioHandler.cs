using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using BaseCleanArchitecture.Application.Features.Usuarios.Commands.UpdateUsuario;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using System.Security.Cryptography;
using System.Text;
using BaseCleanArchitecture.Domain.Resources;
using BC = BCrypt.Net.BCrypt;

namespace BaseCleanArchitecture.Application.Features.Usuarios.Commands.UpdateUsuario
{
    public class UpdateUsuarioHandler : IRequestHandler<UpdateUsuarioCommand, UsuarioUpdateResponseDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public UpdateUsuarioHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioUpdateResponseDto> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
            if (usuario == null)
                throw new Exception(Messages.UsuarioNoEncontrado);

            // Actualizar propiedades básicas
            if (!string.IsNullOrEmpty(request.Usuario.Nombre))
                usuario.Nombre = request.Usuario.Nombre;
            
            if (!string.IsNullOrEmpty(request.Usuario.Apellido))
                usuario.Apellido = request.Usuario.Apellido;
            
            if (!string.IsNullOrEmpty(request.Usuario.Telefono))
                usuario.Telefono = request.Usuario.Telefono;
            
            if (!string.IsNullOrEmpty(request.Usuario.Email))
                usuario.Correo = request.Usuario.Email;
            
            // Remover referencias a entidades eliminadas
            // if (request.Usuario.IdPuesto.HasValue)
            //     usuario.IdPuesto = request.Usuario.IdPuesto.Value;
            
            // if (request.Usuario.IdRol.HasValue)
            //     usuario.IdRol = request.Usuario.IdRol.Value;

            // Nota: La contraseña se actualiza a través del endpoint específico CambiarContraseña

            usuario.DateUpdate = DateTime.UtcNow;
            usuario.IdUserUpdate = request.UserId;

            await _usuarioRepository.UpdateAsync(usuario);
            return _mapper.Map<UsuarioUpdateResponseDto>(usuario);
        }
    }
} 