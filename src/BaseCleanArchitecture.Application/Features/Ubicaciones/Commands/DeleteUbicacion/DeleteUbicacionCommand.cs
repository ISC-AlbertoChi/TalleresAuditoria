using MediatR;

namespace BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.DeleteUbicacion;

public class DeleteUbicacionCommand : IRequest<bool>
{
    public int Id { get; }

    public DeleteUbicacionCommand(int id)
    {
        Id = id;
    }
} 