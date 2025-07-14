using BaseCleanArchitecture.Application.DTOs.Vehiculo;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Vehiculos.Commands.CreateVehiculo
{
    public class CreateVehiculoCommand : IRequest<CreateVehiculoResponseDto>
    {
        public CreateVehiculoDto Vehiculo { get; }
        public int UserId { get; }
        public int IdEmpresa { get; }

        public CreateVehiculoCommand(CreateVehiculoDto vehiculo, int userId, int idEmpresa)
        {
            Vehiculo = vehiculo;
            UserId = userId;
            IdEmpresa = idEmpresa;
        }
    }
} 