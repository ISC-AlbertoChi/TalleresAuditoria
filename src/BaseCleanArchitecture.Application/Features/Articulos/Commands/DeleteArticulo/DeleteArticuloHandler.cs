using BaseCleanArchitecture.Application.DTOs.Articulo;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Articulos.Commands.DeleteArticulo;

public class DeleteArticuloHandler : IRequestHandler<DeleteArticuloCommand, bool>
{
    private readonly IArticuloRepository _articuloRepository;

    public DeleteArticuloHandler(IArticuloRepository articuloRepository)
    {
        _articuloRepository = articuloRepository;
    }

    public async Task<bool> Handle(DeleteArticuloCommand request, CancellationToken cancellationToken)
    {
        var articulo = await _articuloRepository.GetByIdAsync(request.Id);
        if (articulo == null)
            throw new Exception($"No se encontró el artículo con ID: {request.Id}");

        return await _articuloRepository.DeleteAsync(request.Id);
    }
} 