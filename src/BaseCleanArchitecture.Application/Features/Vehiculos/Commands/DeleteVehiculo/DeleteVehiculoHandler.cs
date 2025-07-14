using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Vehiculos.Commands.DeleteVehiculo
{
    public class DeleteVehiculoHandler : IRequestHandler<DeleteVehiculoCommand, bool>
    {
        private readonly IVehiculoRepository _vehiculoRepository;

        public DeleteVehiculoHandler(IVehiculoRepository vehiculoRepository)
        {
            _vehiculoRepository = vehiculoRepository;
        }

        public async Task<bool> Handle(DeleteVehiculoCommand request, CancellationToken cancellationToken)
        {
            var vehiculo = await _vehiculoRepository.GetByIdAsync(request.Id);
            if (vehiculo == null)
                return false;

            await _vehiculoRepository.DeleteAsync(request.Id);
            return true;
        }
    }
} 