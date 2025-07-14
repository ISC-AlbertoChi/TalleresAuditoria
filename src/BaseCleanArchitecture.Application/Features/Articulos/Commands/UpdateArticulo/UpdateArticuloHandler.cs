using AutoMapper;
using BaseCleanArchitecture.Application.DTOs.Articulo;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Articulos.Commands.UpdateArticulo;

public class UpdateArticuloHandler : IRequestHandler<UpdateArticuloCommand, UpdateArticuloResponseDto>
{
    private readonly IArticuloRepository _articuloRepository;
    private readonly IMarcaRepository _marcaRepository;
    private readonly IMapper _mapper;

    public UpdateArticuloHandler(
        IArticuloRepository articuloRepository,
        IMarcaRepository marcaRepository,
        IMapper mapper)
    {
        _articuloRepository = articuloRepository;
        _marcaRepository = marcaRepository;
        _mapper = mapper;
    }

    public async Task<UpdateArticuloResponseDto> Handle(UpdateArticuloCommand request, CancellationToken cancellationToken)
    {
        // Verificar si existe el artículo
        var articuloExistente = await _articuloRepository.GetByIdAsync(request.Id);
        if (articuloExistente == null)
        {
            throw new Exception($"No se encontró el artículo con ID {request.Id}.");
        }

        // Verificar si la marca existe
        var marca = await _marcaRepository.GetByIdAsync(request.Articulo.IdMarca);
        if (marca == null)
        {
            throw new Exception($"No se encontró la marca con ID {request.Articulo.IdMarca}.");
        }

        // Verificar si ya existe otro artículo con el mismo nombre y marca
        var articulosExistentes = await _articuloRepository.GetAllAsync();
        var articuloDuplicado = articulosExistentes.FirstOrDefault(a => 
            a.Nombre == request.Articulo.Nombre && 
            a.IdMarca == request.Articulo.IdMarca &&
            a.Id != request.Id);
            
        if (articuloDuplicado != null)
        {
            throw new Exception("Ya existe un artículo con el mismo nombre y marca.");
        }

        // Mapear el DTO a la entidad
        _mapper.Map(request.Articulo, articuloExistente);
        articuloExistente.IdUserUpdate = request.UserId;
        articuloExistente.DateUpdate = DateTime.UtcNow;

        // Actualizar el artículo
        var articuloActualizado = await _articuloRepository.UpdateAsync(articuloExistente);

        // Mapear la respuesta
        return _mapper.Map<UpdateArticuloResponseDto>(articuloActualizado);
    }
} 