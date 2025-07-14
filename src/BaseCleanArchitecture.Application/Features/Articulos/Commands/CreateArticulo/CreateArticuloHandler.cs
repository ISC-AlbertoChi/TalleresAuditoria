using AutoMapper;
using BaseCleanArchitecture.Application.DTOs.Articulo;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;

namespace BaseCleanArchitecture.Application.Features.Articulos.Commands.CreateArticulo;

public class CreateArticuloHandler : IRequestHandler<CreateArticuloCommand, CreateArticuloResponseDto>
{
    private readonly IArticuloRepository _articuloRepository;
    private readonly IMarcaRepository _marcaRepository;
    private readonly IModeloRepository _modeloRepository;
    private readonly IMapper _mapper;

    public CreateArticuloHandler(
        IArticuloRepository articuloRepository,
        IMarcaRepository marcaRepository,
        IModeloRepository modeloRepository,
        IMapper mapper)
    {
        _articuloRepository = articuloRepository;
        _marcaRepository = marcaRepository;
        _modeloRepository = modeloRepository;
        _mapper = mapper;
    }

    public async Task<CreateArticuloResponseDto> Handle(CreateArticuloCommand request, CancellationToken cancellationToken)
    {
        // Verificar si la marca existe
        var marca = await _marcaRepository.GetByIdAsync(request.Articulo.IdMarca);
        if (marca == null)
        {
            throw new Exception($"No se encontró la marca con ID {request.Articulo.IdMarca}.");
        }

        // Verificar si ya existe un artículo con el mismo nombre
        var articulosExistentes = await _articuloRepository.GetAllAsync();
        var articuloExistente = articulosExistentes.FirstOrDefault(a => 
            a.Nombre == request.Articulo.Nombre && 
            a.IdMarca == request.Articulo.IdMarca);
            
        if (articuloExistente != null)
        {
            throw new Exception("Ya existe un artículo con el mismo nombre y marca.");
        }

        // Mapear el DTO a la entidad
        var articulo = _mapper.Map<Domain.Entities.Articulo>(request.Articulo);
        articulo.IdMarca = request.Articulo.IdMarca;
        articulo.IdModelo = request.Articulo.IdModelo;
        articulo.IdUser = request.UserId;
        articulo.DateCreate = DateTime.UtcNow;
        articulo.DateUpdate = null;
        articulo.IdUserUpdate = null;
        articulo.Status = true;

        // Guardar el artículo
        var articuloCreado = await _articuloRepository.AddAsync(articulo);

        // Mapear la respuesta
        return _mapper.Map<CreateArticuloResponseDto>(articuloCreado);
    }
} 