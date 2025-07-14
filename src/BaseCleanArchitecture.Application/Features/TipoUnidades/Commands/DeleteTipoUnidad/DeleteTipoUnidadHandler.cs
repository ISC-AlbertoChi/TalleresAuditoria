using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.DeleteTipoUnidad;

public class DeleteTipoUnidadHandler : IRequestHandler<DeleteTipoUnidadCommand, bool>
{
    private readonly ITipoUnidadRepository _tipoUnidadRepository;

    public DeleteTipoUnidadHandler(ITipoUnidadRepository tipoUnidadRepository)
    {
        _tipoUnidadRepository = tipoUnidadRepository;
    }

    public async Task<bool> Handle(DeleteTipoUnidadCommand request, CancellationToken cancellationToken)
    {
        var tipoUnidad = await _tipoUnidadRepository.GetByIdAsync(request.Id);
        if (tipoUnidad == null)
            throw new Exception("Tipo de unidad no encontrado");

        return await _tipoUnidadRepository.DeleteAsync(request.Id);
    }
} 