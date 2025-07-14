using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Sucursales.Commands.DeleteSucursal;

public class DeleteSucursalHandler : IRequestHandler<DeleteSucursalCommand, bool>
{
    private readonly ISucursalRepository _sucursalRepository;

    public DeleteSucursalHandler(ISucursalRepository sucursalRepository)
    {
        _sucursalRepository = sucursalRepository;
    }

    public async Task<bool> Handle(DeleteSucursalCommand request, CancellationToken cancellationToken)
    {
        var sucursal = await _sucursalRepository.GetByIdAsync(request.Id);
        if (sucursal == null)
            throw new Exception("Sucursal no encontrada");

        return await _sucursalRepository.DeleteAsync(request.Id);
    }
} 