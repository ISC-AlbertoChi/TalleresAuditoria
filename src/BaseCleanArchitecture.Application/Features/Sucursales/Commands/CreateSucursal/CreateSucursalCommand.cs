using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Sucursales.Commands.CreateSucursal;

public class CreateSucursalCommand : IRequest<CreateSucursalResponseDto>
{
    public CreateSucursalDto Sucursal { get; }
    public int UserId { get; }
    public int IdEmpresa { get; }

    public CreateSucursalCommand(CreateSucursalDto sucursal, int userId, int idEmpresa)
    {
        Sucursal = sucursal;
        UserId = userId;
        IdEmpresa = idEmpresa;
    }
} 