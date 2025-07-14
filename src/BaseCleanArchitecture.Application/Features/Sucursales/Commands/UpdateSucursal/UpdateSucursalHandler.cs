using BaseCleanArchitecture.Application.DTOs;
using AutoMapper;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Sucursales.Commands.UpdateSucursal;

public class UpdateSucursalHandler : IRequestHandler<UpdateSucursalCommand, UpdateSucursalResponseDto>
{
    private readonly ISucursalRepository _sucursalRepository;
    private readonly IMapper _mapper;

    public UpdateSucursalHandler(ISucursalRepository sucursalRepository, IMapper mapper)
    {
        _sucursalRepository = sucursalRepository;
        _mapper = mapper;
    }

    public async Task<UpdateSucursalResponseDto> Handle(UpdateSucursalCommand request, CancellationToken cancellationToken)
    {
        var sucursal = await _sucursalRepository.GetByIdAsync(request.Id);
        if (sucursal == null)
            throw new Exception("Sucursal no encontrada");

        _mapper.Map(request.Sucursal, sucursal);
        sucursal.DateUpdate = DateTime.UtcNow;
        sucursal.IdUserUpdate = request.UserId;

        var updatedSucursal = await _sucursalRepository.UpdateAsync(sucursal);
        return _mapper.Map<UpdateSucursalResponseDto>(updatedSucursal);
    }
} 