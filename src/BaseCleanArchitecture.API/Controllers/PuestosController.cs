using AutoMapper;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BaseCleanArchitecture.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PuestosController : ControllerBase
{
    private readonly IPuestoRepository _puestoRepository;
    private readonly IMapper _mapper;

    public PuestosController(IPuestoRepository puestoRepository, IMapper mapper)
    {
        _puestoRepository = puestoRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Endpoint universal para obtener puestos con diferentes opciones
    /// </summary>
    /// <param name="id">ID específico del puesto (opcional)</param>
    /// <param name="page">Número de página para paginación (opcional)</param>
    /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
    /// <param name="search">Término de búsqueda en nombre (opcional)</param>
    /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
    /// <param name="status">Filtrar por status (opcional)</param>
    /// <returns>Lista de puestos o puesto específico</returns>
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
            // Si se especifica un ID, retornar ese puesto específico
            if (id.HasValue)
            {
                var puesto = await _puestoRepository.GetByIdAsync(id.Value);
                if (puesto == null)
                    return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Puesto no encontrado" });

                return Ok(new ApiResponse<object> { Success = true, Data = puesto });
            }

            // Si es para combo, retornar solo ID y Nombre
            if (combo)
            {
                var puestosCombo = await _puestoRepository.GetAllAsync();
                var comboData = puestosCombo
                    .Where(p => !status.HasValue || p.Status == status.Value)
                    .Select(p => new { p.Id, p.Nombre })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }

            // Obtener todos los puestos con filtros
            var query = await _puestoRepository.GetAllAsync();
            var puestos = query.AsQueryable();

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                puestos = puestos.Where(p => p.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Aplicar filtro de status
            if (status.HasValue)
            {
                puestos = puestos.Where(p => p.Status == status.Value);
            }

            // Aplicar paginación si se especifica
            if (page.HasValue)
            {
                var totalCount = puestos.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedData = puestos
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
            var allData = puestos.ToList();
            return Ok(new ApiResponse<object> { Success = true, Data = allData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Obtener puestos para combobox (solo ID y Nombre)
    /// </summary>
    /// <returns>Lista de puestos con ID y Nombre</returns>
    [HttpGet("combo")]
    public async Task<ActionResult<ApiResponse<object>>> GetCombo()
    {
        try
        {
            var puestos = await _puestoRepository.GetAllAsync();
            var comboData = puestos
                .Where(p => p.Status)
                .Select(p => new { p.Id, p.Nombre })
                .ToList();

            return Ok(new ApiResponse<object> { Success = true, Data = comboData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }
} 