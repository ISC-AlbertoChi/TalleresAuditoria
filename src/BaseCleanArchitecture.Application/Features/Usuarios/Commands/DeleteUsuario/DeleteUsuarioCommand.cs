using MediatR;
using BaseCleanArchitecture.Application.DTOs;

namespace BaseCleanArchitecture.Application.Features.Usuarios.Commands.DeleteUsuario
{
    public class DeleteUsuarioCommand : IRequest<UsuarioResponseDto>
    {
        public int Id { get; set; }
    }
} 