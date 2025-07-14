using MediatR;

namespace BaseCleanArchitecture.Application.Features.Vehiculos.Commands.DeleteVehiculo
{
    public class DeleteVehiculoCommand : IRequest<bool>
    {
        public int Id { get; }

        public DeleteVehiculoCommand(int id)
        {
            Id = id;
        }
    }
} 