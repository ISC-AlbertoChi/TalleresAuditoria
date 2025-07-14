using BaseCleanArchitecture.Application.DTOs;
using AutoMapper;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.CreateTipoUnidad;

public class CreateTipoUnidadHandler : IRequestHandler<CreateTipoUnidadCommand, CreateTipoUnidadResponseDto>
{
    private readonly ITipoUnidadRepository _tipoUnidadRepository;
    private readonly IMapper _mapper;

    public CreateTipoUnidadHandler(ITipoUnidadRepository tipoUnidadRepository, IMapper mapper)
    {
        _tipoUnidadRepository = tipoUnidadRepository;
        _mapper = mapper;
    }

    public async Task<CreateTipoUnidadResponseDto> Handle(CreateTipoUnidadCommand request, CancellationToken cancellationToken)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.TipoUnidad.Clave))
                throw new Exception("La clave del tipo de unidad es requerida");

            // Validar que no exista un tipo de unidad con la misma clave en la misma empresa
            var existingTiposUnidad = await _tipoUnidadRepository.GetAllAsync();
            if (existingTiposUnidad.Any(t => 
                t.Clave.Equals(request.TipoUnidad.Clave, StringComparison.OrdinalIgnoreCase) &&
                (t.IdEmpresa == request.IdEmpresa || t.IdEmpresa == null)))
                throw new Exception("Ya existe un tipo de unidad con esa clave en esta empresa");

            var tipoUnidad = _mapper.Map<Domain.Entities.TipoUnidad>(request.TipoUnidad);
            tipoUnidad.IdEmpresa = request.IdEmpresa;
            tipoUnidad.IdUser = request.UserId;
            tipoUnidad.Status = true;
            tipoUnidad.DateCreate = DateTime.UtcNow;
            tipoUnidad.DateUpdate = null;
            tipoUnidad.IdUserUpdate = null;

            var createdTipoUnidad = await _tipoUnidadRepository.CreateAsync(tipoUnidad);
            return _mapper.Map<CreateTipoUnidadResponseDto>(createdTipoUnidad);
        }
        catch (Exception ex)
        {
            throw new Exception($"Error al crear el tipo de unidad: {ex.Message}", ex);
        }
    }
} 