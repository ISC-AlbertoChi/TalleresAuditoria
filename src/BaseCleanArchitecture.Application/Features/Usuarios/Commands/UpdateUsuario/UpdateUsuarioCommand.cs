using MediatR;
using BaseCleanArchitecture.Application.DTOs;

namespace BaseCleanArchitecture.Application.Features.Usuarios.Commands.UpdateUsuario
{
    public class UpdateUsuarioCommand : IRequest<UsuarioUpdateResponseDto>
    {
        public int Id { get; }
        public UpdateUsuarioDto Usuario { get; }
        public int UserId { get; }

        public UpdateUsuarioCommand(int id, UpdateUsuarioDto usuario, int userId)
        {
            Id = id;
            Usuario = usuario;
            UserId = userId;
        }
    }
} 