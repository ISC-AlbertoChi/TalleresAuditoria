using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Almacenes.Commands.CreateAlmacen;

public class CreateAlmacenCommand : IRequest<CreateAlmacenResponseDto>
{
    public CreateAlmacenDto Almacen { get; }
    public int UserId { get; }
    public int IdEmpresa { get; }

    public CreateAlmacenCommand(CreateAlmacenDto almacen, int userId, int idEmpresa)
    {
        Almacen = almacen;
        UserId = userId;
        IdEmpresa = idEmpresa;
    }
} 