using BaseCleanArchitecture.Application.DTOs.Ubicacion;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.UpdateUbicacion;

public class UpdateUbicacionCommand : IRequest<UpdateUbicacionResponseDto>
{
    public int Id { get; }
    public UpdateUbicacionDto Ubicacion { get; }
    public int UserId { get; }

    public UpdateUbicacionCommand(int id, UpdateUbicacionDto ubicacion, int userId)
    {
        Id = id;
        Ubicacion = ubicacion;
        UserId = userId;
    }
} 