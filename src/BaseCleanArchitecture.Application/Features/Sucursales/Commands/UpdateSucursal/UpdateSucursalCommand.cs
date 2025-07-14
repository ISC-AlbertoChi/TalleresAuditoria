using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Sucursales.Commands.UpdateSucursal;

public class UpdateSucursalCommand : IRequest<UpdateSucursalResponseDto>
{
    public int Id { get; }
    public UpdateSucursalDto Sucursal { get; }
    public int UserId { get; }

    public UpdateSucursalCommand(int id, UpdateSucursalDto sucursal, int userId)
    {
        Id = id;
        Sucursal = sucursal;
        UserId = userId;
    }
} 