using BaseCleanArchitecture.Application.DTOs.Ubicacion;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.CreateUbicacion;

public class CreateUbicacionCommand : IRequest<CreateUbicacionResponseDto>
{
    public CreateUbicacionDto Ubicacion { get; }
    public int UserId { get; }
    public int IdEmpresa { get; }

    public CreateUbicacionCommand(CreateUbicacionDto ubicacion, int userId, int idEmpresa)
    {
        Ubicacion = ubicacion;
        UserId = userId;
        IdEmpresa = idEmpresa;
    }
} 