using AutoMapper;
using BaseCleanArchitecture.Application.Features.Articulos.Commands.CreateArticulo;
using BaseCleanArchitecture.Application.Features.Articulos.Commands.UpdateArticulo;
using BaseCleanArchitecture.Application.Features.Articulos.Commands.DeleteArticulo;
using BaseCleanArchitecture.Application.DTOs.Articulo;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using BaseCleanArchitecture.Application.DTOs;

namespace BaseCleanArchitecture.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ArticuloController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IArticuloRepository _articuloRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ArticuloController(
        IMediator mediator,
        IArticuloRepository articuloRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _articuloRepository = articuloRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Endpoint universal para obtener artículos con diferentes opciones
    /// </summary>
    /// <param name="id">ID específico del artículo (opcional)</param>
    /// <param name="page">Número de página para paginación (opcional)</param>
    /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
    /// <param name="search">Término de búsqueda en nombre (opcional)</param>
    /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
    /// <param name="status">Filtrar por status (opcional)</param>
    /// <returns>Lista de artículos o artículo específico</returns>
    [HttpGet]
    public async Task<ActionResult<ApiResponse<object>>> Get(
        [FromQuery] int? id = null,
        [FromQuery] int? page = null,
        [FromQuery] int pageSize = 10,
        [FromQuery] string? search = null,
        [FromQuery] bool combo = false,
        [FromQuery] bool? status = null)
    {
        try
        {
            // Si se especifica un ID, retornar ese artículo específico
            if (id.HasValue)
            {
                var articulo = await _articuloRepository.GetByIdAsync(id.Value);
                if (articulo == null)
                    return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Artículo no encontrado" });

                return Ok(new ApiResponse<object> { Success = true, Data = articulo });
            }

            // Si es para combo, retornar solo ID y Nombre
            if (combo)
            {
                var articulosCombo = await _articuloRepository.GetAllAsync();
                var comboData = articulosCombo
                    .Where(a => !status.HasValue || a.Status == status.Value)
                    .Select(a => new { a.Id, a.Nombre })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }

            // Obtener todos los artículos con filtros
            var query = await _articuloRepository.GetAllAsync();
            var articulos = query.AsQueryable();

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                articulos = articulos.Where(a => a.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Aplicar filtro de status
            if (status.HasValue)
            {
                articulos = articulos.Where(a => a.Status == status.Value);
            }

            // Aplicar paginación si se especifica
            if (page.HasValue)
            {
                var totalCount = articulos.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedData = articulos
                    .Skip((page.Value - 1) * pageSize)
                    .Take(pageSize)
                    .ToList();

                var result = new
                {
                    Data = pagedData,
                    Pagination = new
                    {
                        CurrentPage = page.Value,
                        PageSize = pageSize,
                        TotalCount = totalCount,
                        TotalPages = totalPages
                    }
                };

                return Ok(new ApiResponse<object> { Success = true, Data = result });
            }

            // Retornar todos sin paginación
            var allData = articulos.ToList();
            return Ok(new ApiResponse<object> { Success = true, Data = allData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Crear un nuevo artículo
    /// </summary>
    /// <param name="articulo">Datos del artículo a crear</param>
    /// <returns>Artículo creado</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateArticuloDto articulo)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Datos de entrada inválidos"));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado"));
            }

            var userId = int.Parse(userIdClaim.Value);
            var command = new CreateArticuloCommand(articulo, userId);
            var result = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(Create), new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Artículo creado correctamente",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear el artículo", ex.Message));
        }
    }

    /// <summary>
    /// Actualizar un artículo existente
    /// </summary>
    /// <param name="id">ID del artículo</param>
    /// <param name="articulo">Datos actualizados del artículo</param>
    /// <returns>Artículo actualizado</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateArticuloDto articulo)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Datos de entrada inválidos"));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado"));
            }

            var userId = int.Parse(userIdClaim.Value);
            var command = new UpdateArticuloCommand(id, articulo, userId);
            var result = await _mediator.Send(command);
            
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Artículo actualizado correctamente",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar el artículo", ex.Message));
        }
    }

    /// <summary>
    /// Eliminar un artículo
    /// </summary>
    /// <param name="id">ID del artículo a eliminar</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteArticuloCommand(id);
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Artículo no encontrado"));
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Artículo eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar el artículo", ex.Message));
        }
    }
} 