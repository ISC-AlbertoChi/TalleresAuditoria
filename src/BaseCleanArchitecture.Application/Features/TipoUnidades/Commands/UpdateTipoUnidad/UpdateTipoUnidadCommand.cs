using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.UpdateTipoUnidad;

public class UpdateTipoUnidadCommand : IRequest<UpdateTipoUnidadResponseDto>
{
    public int Id { get; }
    public UpdateTipoUnidadDto TipoUnidad { get; }
    public int UserId { get; }

    public UpdateTipoUnidadCommand(int id, UpdateTipoUnidadDto tipoUnidad, int userId)
    {
        Id = id;
        TipoUnidad = tipoUnidad;
        UserId = userId;
    }
} 