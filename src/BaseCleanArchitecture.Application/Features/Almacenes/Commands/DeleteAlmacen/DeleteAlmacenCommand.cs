using MediatR;

namespace BaseCleanArchitecture.Application.Features.Almacenes.Commands.DeleteAlmacen;

public class DeleteAlmacenCommand : IRequest<bool>
{
    public int Id { get; }

    public DeleteAlmacenCommand(int id)
    {
        Id = id;
    }
} 