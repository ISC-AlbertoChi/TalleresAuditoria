using MediatR;
using BaseCleanArchitecture.Application.DTOs;

namespace BaseCleanArchitecture.Application.Features.Usuarios.Commands.CreateUsuario
{
    public class CreateUsuarioCommand : IRequest<UsuarioResponseDto>
    {
        public CreateUsuarioDto Usuario { get; }

        public CreateUsuarioCommand(CreateUsuarioDto usuario)
        {
            Usuario = usuario;
        }
    }
} 