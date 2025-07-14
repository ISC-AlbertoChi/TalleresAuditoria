using MediatR;

namespace BaseCleanArchitecture.Application.Features.Articulos.Commands.DeleteArticulo;

public class DeleteArticuloCommand : IRequest<bool>
{
    public int Id { get; }

    public DeleteArticuloCommand(int id)
    {
        Id = id;
    }
} 