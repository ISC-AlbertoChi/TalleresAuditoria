using BaseCleanArchitecture.Application.Features.Marcas.Commands.DeleteMarca;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Marcas.Commands.DeleteMarca
{
    public class DeleteMarcaHandler : IRequestHandler<DeleteMarcaCommand, bool>
    {
        private readonly IMarcaRepository _marcaRepository;

        public DeleteMarcaHandler(IMarcaRepository marcaRepository)
        {
            _marcaRepository = marcaRepository;
        }

        public async Task<bool> Handle(DeleteMarcaCommand request, CancellationToken cancellationToken)
        {
            var marca = await _marcaRepository.GetByIdAsync(request.Id);
            if (marca == null)
                return false;

            return await _marcaRepository.DeleteAsync(request.Id);
        }
    }
} 