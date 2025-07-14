using AutoMapper;
using BaseCleanArchitecture.Application.DTOs.Ubicacion;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.UpdateUbicacion;

public class UpdateUbicacionHandler : IRequestHandler<UpdateUbicacionCommand, UpdateUbicacionResponseDto>
{
    private readonly IUbicacionRepository _ubicacionRepository;
    private readonly IMapper _mapper;

    public UpdateUbicacionHandler(IUbicacionRepository ubicacionRepository, IMapper mapper)
    {
        _ubicacionRepository = ubicacionRepository;
        _mapper = mapper;
    }

    public async Task<UpdateUbicacionResponseDto> Handle(UpdateUbicacionCommand request, CancellationToken cancellationToken)
    {
        // Verificar si existe la ubicación
        var ubicacionExistente = await _ubicacionRepository.GetByIdAsync(request.Id);
        if (ubicacionExistente == null)
            throw new Exception($"No se encontró la ubicación con ID {request.Id}.");

        // Verificar si ya existe otra ubicación con la misma clave en el mismo almacén
        if (await _ubicacionRepository.ExistsByClaveAndAlmacenAsync(request.Ubicacion.Clave, request.Ubicacion.IdAlmacen) &&
            ubicacionExistente.Clave != request.Ubicacion.Clave)
            throw new Exception($"Ya existe una ubicación con la clave '{request.Ubicacion.Clave}' en el almacén especificado.");

        // Mapear el DTO a la entidad
        var ubicacion = _mapper.Map<Domain.Entities.Ubicacion>(request.Ubicacion);
        ubicacion.Id = request.Id;
        ubicacion.IdUser = ubicacionExistente.IdUser;
        ubicacion.DateCreate = ubicacionExistente.DateCreate;
        ubicacion.Status = ubicacionExistente.Status;
        ubicacion.IdUserUpdate = request.UserId;
        ubicacion.DateUpdate = DateTime.UtcNow;

        // Actualizar la ubicación
        var ubicacionActualizada = await _ubicacionRepository.UpdateAsync(ubicacion);

        // Mapear la respuesta
        return _mapper.Map<UpdateUbicacionResponseDto>(ubicacionActualizada);
    }
} 