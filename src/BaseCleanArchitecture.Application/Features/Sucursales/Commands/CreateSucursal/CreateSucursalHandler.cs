using BaseCleanArchitecture.Application.DTOs;
using AutoMapper;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Sucursales.Commands.CreateSucursal;

public class CreateSucursalHandler : IRequestHandler<CreateSucursalCommand, CreateSucursalResponseDto>
{
    private readonly ISucursalRepository _sucursalRepository;
    private readonly IMapper _mapper;

    public CreateSucursalHandler(
        ISucursalRepository sucursalRepository,
        IMapper mapper)
    {
        _sucursalRepository = sucursalRepository;
        _mapper = mapper;
    }

    public async Task<CreateSucursalResponseDto> Handle(CreateSucursalCommand request, CancellationToken cancellationToken)
    {
        var sucursal = _mapper.Map<Domain.Entities.Sucursal>(request.Sucursal);
        sucursal.IdUser = request.UserId; // Auditoría: quién crea
        sucursal.IdEmpresa = request.IdEmpresa; // Empresa del token
        sucursal.Status = true;
        sucursal.DateCreate = DateTime.UtcNow; // Auditoría: cuándo se crea

        var createdSucursal = await _sucursalRepository.CreateAsync(sucursal);
        return _mapper.Map<CreateSucursalResponseDto>(createdSucursal);
    }
} 