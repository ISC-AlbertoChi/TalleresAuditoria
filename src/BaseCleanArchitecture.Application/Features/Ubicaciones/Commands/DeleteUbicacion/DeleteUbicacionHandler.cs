using BaseCleanArchitecture.Application.DTOs.Ubicacion;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.DeleteUbicacion;

public class DeleteUbicacionHandler : IRequestHandler<DeleteUbicacionCommand, bool>
{
    private readonly IUbicacionRepository _ubicacionRepository;

    public DeleteUbicacionHandler(IUbicacionRepository ubicacionRepository)
    {
        _ubicacionRepository = ubicacionRepository;
    }

    public async Task<bool> Handle(DeleteUbicacionCommand request, CancellationToken cancellationToken)
    {
        // Verificar si existe la ubicación
        var ubicacion = await _ubicacionRepository.GetByIdAsync(request.Id);
        if (ubicacion == null)
            throw new Exception($"No se encontró la ubicación con ID {request.Id}.");

        // Verificar si tiene artículos activos
        if (await _ubicacionRepository.HasArticulosActivosAsync(request.Id))
            throw new Exception("No se puede eliminar la ubicación porque tiene artículos activos asociados.");

        // Realizar la baja lógica
        return await _ubicacionRepository.DeleteAsync(request.Id);
    }
} 