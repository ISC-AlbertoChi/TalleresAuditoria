using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Almacenes.Commands.UpdateAlmacen;

public class UpdateAlmacenCommand : IRequest<UpdateAlmacenResponseDto>
{
    public int Id { get; }
    public UpdateAlmacenDto Almacen { get; }
    public int UserId { get; }

    public UpdateAlmacenCommand(int id, UpdateAlmacenDto almacen, int userId)
    {
        Id = id;
        Almacen = almacen;
        UserId = userId;
    }
} 