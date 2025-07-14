using AutoMapper;
using BaseCleanArchitecture.Application.DTOs.Vehiculo;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Vehiculos.Commands.UpdateVehiculo
{
    public class UpdateVehiculoHandler : IRequestHandler<UpdateVehiculoCommand, UpdateVehiculoResponseDto>
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IMapper _mapper;

        public UpdateVehiculoHandler(IVehiculoRepository vehiculoRepository, IMapper mapper)
        {
            _vehiculoRepository = vehiculoRepository;
            _mapper = mapper;
        }

        public async Task<UpdateVehiculoResponseDto> Handle(UpdateVehiculoCommand request, CancellationToken cancellationToken)
        {
            var existingVehiculo = await _vehiculoRepository.GetByIdAsync(request.Id);
            if (existingVehiculo == null)
                throw new InvalidOperationException("Vehículo no encontrado");

            _mapper.Map(request.Vehiculo, existingVehiculo);
            existingVehiculo.IdUserUpdate = request.UserId;
            existingVehiculo.DateUpdate = DateTime.UtcNow;

            await _vehiculoRepository.UpdateAsync(existingVehiculo);
            var result = existingVehiculo;
            return _mapper.Map<UpdateVehiculoResponseDto>(result);
        }
    }
} 