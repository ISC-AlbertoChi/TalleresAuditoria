using BaseCleanArchitecture.Application.DTOs.Vehiculo;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Vehiculos.Commands.UpdateVehiculo
{
    public class UpdateVehiculoCommand : IRequest<UpdateVehiculoResponseDto>
    {
        public int Id { get; }
        public UpdateVehiculoDto Vehiculo { get; }
        public int UserId { get; }

        public UpdateVehiculoCommand(int id, UpdateVehiculoDto vehiculo, int userId)
        {
            Id = id;
            Vehiculo = vehiculo;
            UserId = userId;
        }
    }
} 