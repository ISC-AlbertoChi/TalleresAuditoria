using MediatR;

namespace BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.DeleteTipoUnidad;

public class DeleteTipoUnidadCommand : IRequest<bool>
{
    public int Id { get; }

    public DeleteTipoUnidadCommand(int id)
    {
        Id = id;
    }
} 