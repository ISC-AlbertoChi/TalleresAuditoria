using AutoMapper;
using BaseCleanArchitecture.Application.DTOs.Ubicacion;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.CreateUbicacion;

public class CreateUbicacionHandler : IRequestHandler<CreateUbicacionCommand, CreateUbicacionResponseDto>
{
    private readonly IUbicacionRepository _ubicacionRepository;
    private readonly IMapper _mapper;

    public CreateUbicacionHandler(IUbicacionRepository ubicacionRepository, IMapper mapper)
    {
        _ubicacionRepository = ubicacionRepository;
        _mapper = mapper;
    }

    public async Task<CreateUbicacionResponseDto> Handle(CreateUbicacionCommand request, CancellationToken cancellationToken)
    {
        // Verificar si ya existe una ubicación con la misma clave en el mismo almacén
        if (await _ubicacionRepository.ExistsByClaveAndAlmacenAsync(request.Ubicacion.Clave, request.Ubicacion.IdAlmacen))
            throw new Exception($"Ya existe una ubicación con la clave '{request.Ubicacion.Clave}' en el almacén especificado.");

        // Mapear el DTO a la entidad
        var ubicacion = _mapper.Map<Domain.Entities.Ubicacion>(request.Ubicacion);
        ubicacion.IdUser = request.UserId; // Usuario del token
        ubicacion.IdEmpresa = request.IdEmpresa; // Empresa del token
        ubicacion.Status = true;
        ubicacion.DateCreate = DateTime.UtcNow;
        ubicacion.DateUpdate = null; // Solo se actualiza en PUT
        ubicacion.IdUserUpdate = null; // Solo se actualiza en PUT

        // Guardar la ubicación
        var ubicacionCreada = await _ubicacionRepository.AddAsync(ubicacion);

        // Mapear la respuesta
        return _mapper.Map<CreateUbicacionResponseDto>(ubicacionCreada);
    }
} 