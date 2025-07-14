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
public class RolesController : ControllerBase
{
    private readonly IRolRepository _rolRepository;
    private readonly IMapper _mapper;

    public RolesController(IRolRepository rolRepository, IMapper mapper)
    {
        _rolRepository = rolRepository;
        _mapper = mapper;
    }

    /// <summary>
    /// Endpoint universal para obtener roles con diferentes opciones
    /// </summary>
    /// <param name="id">ID específico del rol (opcional)</param>
    /// <param name="page">Número de página para paginación (opcional)</param>
    /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
    /// <param name="search">Término de búsqueda en nombre (opcional)</param>
    /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
    /// <param name="status">Filtrar por status (opcional)</param>
    /// <returns>Lista de roles o rol específico</returns>
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
            // Si se especifica un ID, retornar ese rol específico
            if (id.HasValue)
            {
                var rol = await _rolRepository.GetByIdAsync(id.Value);
                if (rol == null)
                    return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Rol no encontrado" });

                return Ok(new ApiResponse<object> { Success = true, Data = rol });
            }

            // Si es para combo, retornar solo ID y Nombre
            if (combo)
            {
                var rolesCombo = await _rolRepository.GetAllAsync();
                var comboData = rolesCombo
                    .Where(r => !status.HasValue || r.Status == status.Value)
                    .Select(r => new { r.Id, r.Nombre })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }

            // Obtener todos los roles con filtros
            var query = await _rolRepository.GetAllAsync();
            var roles = query.AsQueryable();

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                roles = roles.Where(r => r.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Aplicar filtro de status
            if (status.HasValue)
            {
                roles = roles.Where(r => r.Status == status.Value);
            }

            // Aplicar paginación si se especifica
            if (page.HasValue)
            {
                var totalCount = roles.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedData = roles
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
            var allData = roles.ToList();
            return Ok(new ApiResponse<object> { Success = true, Data = allData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Obtener roles para combobox (solo ID y Nombre)
    /// </summary>
    /// <returns>Lista de roles con ID y Nombre</returns>
    [HttpGet("combo")]
    public async Task<ActionResult<ApiResponse<object>>> GetCombo()
    {
        try
        {
            var roles = await _rolRepository.GetAllAsync();
            var comboData = roles
                .Where(r => r.Status)
                .Select(r => new { r.Id, r.Nombre })
                .ToList();

            return Ok(new ApiResponse<object> { Success = true, Data = comboData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }
} 