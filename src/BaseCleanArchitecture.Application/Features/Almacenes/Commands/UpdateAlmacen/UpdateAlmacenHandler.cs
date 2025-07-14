using AutoMapper;
using BaseCleanArchitecture.Application.Features.Almacenes.Commands.UpdateAlmacen;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Almacenes.Commands.UpdateAlmacen;

public class UpdateAlmacenHandler : IRequestHandler<UpdateAlmacenCommand, UpdateAlmacenResponseDto>
{
    private readonly IAlmacenRepository _almacenRepository;
    private readonly IMapper _mapper;

    public UpdateAlmacenHandler(IAlmacenRepository almacenRepository, IMapper mapper)
    {
        _almacenRepository = almacenRepository;
        _mapper = mapper;
    }

    public async Task<UpdateAlmacenResponseDto> Handle(UpdateAlmacenCommand request, CancellationToken cancellationToken)
    {
        var almacen = await _almacenRepository.GetByIdAsync(request.Id);
        if (almacen == null)
            throw new Exception($"No se encontró el almacén con ID: {request.Id}");

        // NO validar por empresa - ignorar completamente
        if (!string.IsNullOrEmpty(request.Almacen.Nombre) && 
            await _almacenRepository.ExistsByNombreAndSucursalAsync(request.Almacen.Nombre, almacen.IdSucursal) && 
            request.Almacen.Nombre != almacen.Nombre)
            throw new Exception($"Ya existe un almacén con el nombre {request.Almacen.Nombre} en esta sucursal");

        _mapper.Map(request.Almacen, almacen);
        almacen.DateUpdate = DateTime.UtcNow;
        almacen.IdUserUpdate = request.UserId;

        var updatedAlmacen = await _almacenRepository.UpdateAsync(almacen);
        return _mapper.Map<UpdateAlmacenResponseDto>(updatedAlmacen);
    }
} 