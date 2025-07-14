using AutoMapper;
using BaseCleanArchitecture.Application.DTOs.Vehiculo;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Vehiculos.Commands.CreateVehiculo
{
    public class CreateVehiculoHandler : IRequestHandler<CreateVehiculoCommand, CreateVehiculoResponseDto>
    {
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IMarcaRepository _marcaRepository;
        private readonly IModeloRepository _modeloRepository;
        private readonly IMapper _mapper;

        public CreateVehiculoHandler(
            IVehiculoRepository vehiculoRepository,
            IMarcaRepository marcaRepository,
            IModeloRepository modeloRepository,
            IMapper mapper)
        {
            _vehiculoRepository = vehiculoRepository;
            _marcaRepository = marcaRepository;
            _modeloRepository = modeloRepository;
            _mapper = mapper;
        }

        public async Task<CreateVehiculoResponseDto> Handle(CreateVehiculoCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Vehiculo.Placa))
                    throw new Exception("La placa del vehículo es requerida");

                // Validar que no exista un vehículo con la misma placa en la misma empresa
                var existingVehiculos = await _vehiculoRepository.GetAllAsync();
                if (existingVehiculos.Any(v => 
                    v.Placa.Equals(request.Vehiculo.Placa, StringComparison.OrdinalIgnoreCase) &&
                    (v.IdEmpresa == request.IdEmpresa || v.IdEmpresa == null)))
                    throw new Exception("Ya existe un vehículo con esa placa en esta empresa");

                var dto = request.Vehiculo;

                // Usar directamente los IDs
                int? idMarca = dto.IdMarca;
                int? idModelo = dto.IdModelo;

                var vehiculo = _mapper.Map<Vehiculo>(dto);
                vehiculo.IdMarca = idMarca;
                vehiculo.IdModelo = idModelo;
                vehiculo.IdEmpresa = request.IdEmpresa;
                vehiculo.IdUser = request.UserId;
                vehiculo.Status = true;
                vehiculo.DateCreate = DateTime.UtcNow;
                vehiculo.DateUpdate = null;
                vehiculo.IdUserUpdate = null;

                var result = await _vehiculoRepository.CreateAsync(vehiculo);
                return _mapper.Map<CreateVehiculoResponseDto>(result);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear el vehículo: {ex.Message}", ex);
            }
        }
    }
} 