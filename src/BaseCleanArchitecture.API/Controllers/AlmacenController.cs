using AutoMapper;
using BaseCleanArchitecture.Application.Features.Almacenes.Commands.CreateAlmacen;
using BaseCleanArchitecture.Application.Features.Almacenes.Commands.UpdateAlmacen;
using BaseCleanArchitecture.Application.Features.Almacenes.Commands.DeleteAlmacen;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaseCleanArchitecture.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AlmacenController : ControllerBase
{
    private readonly IMediator _mediator;
    private readonly IAlmacenRepository _almacenRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AlmacenController(IMediator mediator, IAlmacenRepository almacenRepository, IMapper mapper, IHttpContextAccessor httpContextAccessor)
    {
        _mediator = mediator;
        _almacenRepository = almacenRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    /// <summary>
    /// Endpoint universal para obtener almacenes con diferentes opciones
    /// </summary>
    /// <param name="id">ID específico del almacén (opcional)</param>
    /// <param name="page">Número de página para paginación (opcional)</param>
    /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
    /// <param name="search">Término de búsqueda en nombre (opcional)</param>
    /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
    /// <param name="status">Filtrar por status (opcional)</param>
    /// <returns>Lista de almacenes o almacén específico</returns>
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
            // Si se especifica un ID, retornar ese almacén específico
            if (id.HasValue)
            {
                var almacen = await _almacenRepository.GetByIdAsync(id.Value);
                if (almacen == null)
                    return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Almacén no encontrado" });

                return Ok(new ApiResponse<object> { Success = true, Data = almacen });
            }

            // Si es para combo, retornar solo ID y Nombre
            if (combo)
            {
                var almacenesCombo = await _almacenRepository.GetAllAsync();
                var comboData = almacenesCombo
                    .Where(a => !status.HasValue || a.Status == status.Value)
                    .Select(a => new { a.Id, a.Nombre })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }

            // Obtener todos los almacenes con filtros
            var query = await _almacenRepository.GetAllAsync();
            var almacenes = query.AsQueryable();

            // Aplicar filtro de búsqueda
            if (!string.IsNullOrEmpty(search))
            {
                almacenes = almacenes.Where(a => a.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            // Aplicar filtro de status
            if (status.HasValue)
            {
                almacenes = almacenes.Where(a => a.Status == status.Value);
            }

            // Aplicar paginación si se especifica
            if (page.HasValue)
            {
                var totalCount = almacenes.Count();
                var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                var pagedData = almacenes
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
            var allData = almacenes.ToList();
            return Ok(new ApiResponse<object> { Success = true, Data = allData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Obtener almacenes para combobox (solo ID y Nombre)
    /// </summary>
    /// <returns>Lista de almacenes con ID y Nombre</returns>
    [HttpGet("combo")]
    public async Task<ActionResult<ApiResponse<object>>> GetCombo()
    {
        try
        {
            var almacenes = await _almacenRepository.GetAllAsync();
            var comboData = almacenes
                .Where(a => a.Status)
                .Select(a => new { a.Id, a.Nombre })
                .ToList();

            return Ok(new ApiResponse<object> { Success = true, Data = comboData });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
        }
    }

    /// <summary>
    /// Crear un nuevo almacén
    /// </summary>
    /// <param name="almacen">Datos del almacén a crear</param>
    /// <returns>Almacén creado</returns>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateAlmacenDto almacen)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Datos de entrada inválidos", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            var idEmpresaClaim = User.FindFirst("IdEmpresa");
            
            if (userIdClaim == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado", "Usuario no autenticado"));
            }

            if (idEmpresaClaim == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Empresa no especificada en el token"));
            }

            var userId = int.Parse(userIdClaim.Value);
            var idEmpresa = int.Parse(idEmpresaClaim.Value);
            var command = new CreateAlmacenCommand(almacen, userId, idEmpresa);
            var result = await _mediator.Send(command);
            
            return CreatedAtAction(nameof(Create), new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Almacén creado correctamente",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear el almacén", ex.Message));
        }
    }

    /// <summary>
    /// Actualizar un almacén existente
    /// </summary>
    /// <param name="id">ID del almacén</param>
    /// <param name="almacen">Datos actualizados del almacén</param>
    /// <returns>Almacén actualizado</returns>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] UpdateAlmacenDto almacen)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ApiResponse<object>.ErrorResponse("Datos de entrada inválidos", 
                    ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
            if (userIdClaim == null)
            {
                return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado", "Usuario no autenticado"));
            }

            var userId = int.Parse(userIdClaim.Value);
            var command = new UpdateAlmacenCommand(id, almacen, userId);
            var result = await _mediator.Send(command);
            
            return Ok(new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Almacén actualizado correctamente",
                Data = result
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar el almacén", ex.Message));
        }
    }

    /// <summary>
    /// Eliminar un almacén
    /// </summary>
    /// <param name="id">ID del almacén a eliminar</param>
    /// <returns>Resultado de la eliminación</returns>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            var command = new DeleteAlmacenCommand(id);
            var result = await _mediator.Send(command);
            
            if (!result)
            {
                return NotFound(ApiResponse<object>.ErrorResponse("Almacén no encontrado", "El almacén especificado no existe"));
            }

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Mensaje = "Almacén eliminado correctamente"
            });
        }
        catch (Exception ex)
        {
            return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar el almacén", ex.Message));
        }
    }
} 