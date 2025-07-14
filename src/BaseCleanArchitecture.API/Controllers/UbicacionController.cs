using AutoMapper;
using BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.CreateUbicacion;
using BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.UpdateUbicacion;
using BaseCleanArchitecture.Application.Features.Ubicaciones.Commands.DeleteUbicacion;
using BaseCleanArchitecture.Application.DTOs.Ubicacion;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BaseCleanArchitecture.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class UbicacionController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IUbicacionRepository _ubicacionRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public UbicacionController(
        IMediator mediator,
        IUbicacionRepository ubicacionRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _ubicacionRepository = ubicacionRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Endpoint universal para obtener ubicaciones con diferentes opciones
    /// </summary>
    /// <param name="id">ID específico de la ubicación (opcional)</param>
    /// <param name="page">Número de página para paginación (opcional)</param>
    /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
    /// <param name="search">Término de búsqueda en nombre (opcional)</param>
    /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
    /// <param name="status">Filtrar por status (opcional)</param>
    /// <returns>Lista de ubicaciones o ubicación específica</returns>
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
            // Si se especifica un ID, retornar esa ubicación específica
            if (id.HasValue)
            {
                var ubicacion = await _ubicacionRepository.GetByIdAsync(id.Value);
                if (ubicacion == null)
                    return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Ubicación no encontrada" });

                return Ok(new ApiResponse<object> { Success = true, Data = ubicacion });
            }

            // Si es para combo, retornar solo ID y Nombre
            if (combo)
            {
                var ubicacionesCombo = await _ubicacionRepository.GetAllAsync();
                var comboData = ubicacionesCombo
                    .Where(u => !status.HasValue || u.Status == status.Value)
                    .Select(u => new { u.Id, u.Clave })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }

            // Obtener todas las ubicaciones con filtros
            var query = await _ubicacionRepository.GetAllAsync();
            var ubicaciones = query.AsQueryable();

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                ubicaciones = ubicaciones.Where(u => u.Clave.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Aplicar filtro de status
            if (status.HasValue)
            {
                ubicaciones = ubicaciones.Where(u => u.Status == status.Value);
            }

            // Aplicar paginación si se especifica
            if (page.HasValue)
            {
                var totalCount = ubicaciones.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedData = ubicaciones
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

            // Retornar todas sin paginación
            var allData = ubicaciones.ToList();
            return Ok(new ApiResponse<object> { Success = true, Data = allData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Crear una nueva ubicación
    /// </summary>
    /// <param name="ubicacion">Datos de la ubicación a crear</param>
    /// <returns>Ubicación creada</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateUbicacionDto ubicacion)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Datos de entrada inválidos"));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var idEmpresaClaim = User.FindFirst("IdEmpresa");
            
            if (userIdClaim == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado"));
            }

            if (idEmpresaClaim == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Empresa no especificada en el token"));
            }

            var userId = int.Parse(userIdClaim.Value);
            var idEmpresa = int.Parse(idEmpresaClaim.Value);
            var command = new CreateUbicacionCommand(ubicacion, userId, idEmpresa);
            var result = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(Get), new { id = result.Id }, new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Ubicación creada correctamente",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear la ubicación", ex.Message));
        }
    }

    /// <summary>
    /// Actualizar una ubicación existente
    /// </summary>
    /// <param name="id">ID de la ubicación</param>
    /// <param name="ubicacion">Datos actualizados de la ubicación</param>
    /// <returns>Ubicación actualizada</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateUbicacionDto ubicacion)
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
            var command = new UpdateUbicacionCommand(id, ubicacion, userId);
            var result = await _mediator.Send(command);
            
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Ubicación actualizada correctamente",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar la ubicación", ex.Message));
        }
    }

    /// <summary>
    /// Eliminar una ubicación
    /// </summary>
    /// <param name="id">ID de la ubicación a eliminar</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteUbicacionCommand(id);
            var result = await _mediator.Send(command);
            
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Ubicación eliminada correctamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar la ubicación", ex.Message));
        }
    }
} 