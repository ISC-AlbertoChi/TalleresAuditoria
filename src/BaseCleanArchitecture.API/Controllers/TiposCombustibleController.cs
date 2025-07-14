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
public class TiposCombustibleController : ControllerBase
{
    private readonly ITipoCombustibleRepository _tipoCombustibleRepository;
    private readonly IMapper _mapper;

    public TiposCombustibleController(ITipoCombustibleRepository tipoCombustibleRepository, IMapper mapper)
    {
        _tipoCombustibleRepository = tipoCombustibleRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Endpoint universal para obtener tipos de combustible con diferentes opciones
    /// </summary>
    /// <param name="id">ID específico del tipo de combustible (opcional)</param>
    /// <param name="page">Número de página para paginación (opcional)</param>
    /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
    /// <param name="search">Término de búsqueda en nombre (opcional)</param>
    /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
    /// <param name="status">Filtrar por status (opcional)</param>
    /// <returns>Lista de tipos de combustible o tipo específico</returns>
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
            // Si se especifica un ID, retornar ese tipo de combustible específico
            if (id.HasValue)
            {
                var tipoCombustible = await _tipoCombustibleRepository.GetByIdAsync(id.Value);
                if (tipoCombustible == null)
                    return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Tipo de combustible no encontrado" });

                return Ok(new ApiResponse<object> { Success = true, Data = tipoCombustible });
            }

            // Si es para combo, retornar solo ID y Nombre
            if (combo)
            {
                var tiposCombustibleCombo = await _tipoCombustibleRepository.GetAllAsync();
                var comboData = tiposCombustibleCombo
                    .Where(t => !status.HasValue || t.Status == status.Value)
                    .Select(t => new { t.Id, t.Nombre })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }

            // Obtener todos los tipos de combustible con filtros
            var query = await _tipoCombustibleRepository.GetAllAsync();
            var tiposCombustible = query.AsQueryable();

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                tiposCombustible = tiposCombustible.Where(t => t.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Aplicar filtro de status
            if (status.HasValue)
            {
                tiposCombustible = tiposCombustible.Where(t => t.Status == status.Value);
            }

            // Aplicar paginación si se especifica
            if (page.HasValue)
            {
                var totalCount = tiposCombustible.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedData = tiposCombustible
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
            var allData = tiposCombustible.ToList();
            return Ok(new ApiResponse<object> { Success = true, Data = allData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Obtener tipos de combustible para combobox (solo ID y Nombre)
    /// </summary>
    /// <returns>Lista de tipos de combustible con ID y Nombre</returns>
    [HttpGet("combo")]
    public async Task<ActionResult<ApiResponse<object>>> GetCombo()
    {
        try
        {
            var tiposCombustible = await _tipoCombustibleRepository.GetAllAsync();
            var comboData = tiposCombustible
                .Where(t => t.Status)
                .Select(t => new { t.Id, t.Nombre })
                .ToList();

            return Ok(new ApiResponse<object> { Success = true, Data = comboData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }
} 