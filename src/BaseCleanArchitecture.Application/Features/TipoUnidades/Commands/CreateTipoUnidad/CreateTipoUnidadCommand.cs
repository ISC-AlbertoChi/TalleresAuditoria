using BaseCleanArchitecture.Application.DTOs;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.CreateTipoUnidad;

public class CreateTipoUnidadCommand : IRequest<CreateTipoUnidadResponseDto>
{
    public CreateTipoUnidadDto TipoUnidad { get; }
    public int UserId { get; }
    public int IdEmpresa { get; }

    public CreateTipoUnidadCommand(CreateTipoUnidadDto tipoUnidad, int userId, int idEmpresa)
    {
        TipoUnidad = tipoUnidad;
        UserId = userId;
        IdEmpresa = idEmpresa;
    }
} 