using System.Threading;
using System.Threading.Tasks;
using MediatR;
using AutoMapper;
using BaseCleanArchitecture.Application.Features.Usuarios.Commands.DeleteUsuario;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Domain.Resources;

namespace BaseCleanArchitecture.Application.Features.Usuarios.Commands.DeleteUsuario
{
    public class DeleteUsuarioHandler : IRequestHandler<DeleteUsuarioCommand, UsuarioResponseDto>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IMapper _mapper;

        public DeleteUsuarioHandler(IUsuarioRepository usuarioRepository, IMapper mapper)
        {
            _usuarioRepository = usuarioRepository;
            _mapper = mapper;
        }

        public async Task<UsuarioResponseDto> Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(request.Id);
            if (usuario == null)
                throw new System.Exception(Messages.UsuarioNoEncontrado);

            var hasRelations = await _usuarioRepository.HasActiveRelationsAsync(request.Id);
            if (hasRelations)
                throw new System.Exception(Messages.NoSePuedeEliminar);

            usuario.Status = false;
            usuario.DateUpdate = System.DateTime.UtcNow;
            usuario.IdUserUpdate = request.Id; // Asumiendo que el usuario que da de baja es el mismo

            await _usuarioRepository.UpdateAsync(usuario);
            return _mapper.Map<UsuarioResponseDto>(usuario);
        }
    }
} 