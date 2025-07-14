using MediatR;

namespace BaseCleanArchitecture.Application.Features.Sucursales.Commands.DeleteSucursal;

public class DeleteSucursalCommand : IRequest<bool>
{
    public int Id { get; }

    public DeleteSucursalCommand(int id)
    {
        Id = id;
    }
} 