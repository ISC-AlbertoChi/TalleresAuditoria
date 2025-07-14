using AutoMapper;
using BaseCleanArchitecture.Application.Features.Almacenes.Commands.CreateAlmacen;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Almacenes.Commands.CreateAlmacen;

public class CreateAlmacenHandler : IRequestHandler<CreateAlmacenCommand, CreateAlmacenResponseDto>
{
    private readonly IAlmacenRepository _almacenRepository;
    private readonly IMapper _mapper;

    public CreateAlmacenHandler(IAlmacenRepository almacenRepository, IMapper mapper)
    {
        _almacenRepository = almacenRepository;
        _mapper = mapper;
    }

    public async Task<CreateAlmacenResponseDto> Handle(CreateAlmacenCommand request, CancellationToken cancellationToken)
    {
        // NO validar por empresa - ignorar completamente
        if (!string.IsNullOrEmpty(request.Almacen.Nombre) && 
            await _almacenRepository.ExistsByNombreAndSucursalAsync(request.Almacen.Nombre, request.Almacen.IdSucursal))
            throw new Exception($"Ya existe un almacén con el nombre {request.Almacen.Nombre} en esta sucursal");

        var almacen = _mapper.Map<Domain.Entities.Almacen>(request.Almacen);
        almacen.IdUser = request.UserId; // Usuario del token
        almacen.IdEmpresa = request.IdEmpresa; // Empresa del token
        almacen.Status = true;
        almacen.DateCreate = DateTime.UtcNow;
        almacen.DateUpdate = null; // Solo se actualiza en PUT
        almacen.IdUserUpdate = null; // Solo se actualiza en PUT

        var createdAlmacen = await _almacenRepository.CreateAsync(almacen);
        return _mapper.Map<CreateAlmacenResponseDto>(createdAlmacen);
    }
} 