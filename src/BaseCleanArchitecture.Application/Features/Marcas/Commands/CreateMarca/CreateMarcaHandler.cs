using AutoMapper;
using BaseCleanArchitecture.Application.Features.Marcas.Commands.CreateMarca;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Marcas.Commands.CreateMarca
{
    public class CreateMarcaHandler : IRequestHandler<CreateMarcaCommand, CreateMarcaResponseDto>
    {
        private readonly IMarcaRepository _marcaRepository;
        private readonly IMapper _mapper;

        public CreateMarcaHandler(IMarcaRepository marcaRepository, IMapper mapper)
        {
            _marcaRepository = marcaRepository;
            _mapper = mapper;
        }

        public async Task<CreateMarcaResponseDto> Handle(CreateMarcaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(request.Marca.Nombre))
                    throw new Exception("El nombre de la marca es requerido");

                if (string.IsNullOrWhiteSpace(request.Marca.Clave))
                    throw new Exception("La clave de la marca es requerida");

                // Validar que no exista una marca con la misma clave en la misma empresa
                var existingMarcas = await _marcaRepository.GetAllAsync();
                if (existingMarcas.Any(m => 
                    m.Clave.Equals(request.Marca.Clave, StringComparison.OrdinalIgnoreCase) &&
                    (m.IdEmpresa == request.IdEmpresa || m.IdEmpresa == null)))
                    throw new Exception("Ya existe una marca con esa clave en esta empresa");

                var marca = _mapper.Map<Domain.Entities.Marca>(request.Marca);
                marca.IdEmpresa = request.IdEmpresa;
                marca.IdUser = request.UserId;
                marca.Status = true;
                marca.DateCreate = DateTime.UtcNow;
                marca.DateUpdate = null;
                marca.IdUserUpdate = null;

                var createdMarca = await _marcaRepository.CreateAsync(marca);
                return _mapper.Map<CreateMarcaResponseDto>(createdMarca);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al crear la marca: {ex.Message}", ex);
            }
        }
    }
} 