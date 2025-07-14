using AutoMapper;
using BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.CreateTipoUnidad;
using BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.UpdateTipoUnidad;
using BaseCleanArchitecture.Application.Features.TipoUnidades.Commands.DeleteTipoUnidad;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BaseCleanArchitecture.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class TipoUnidadController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ITipoUnidadRepository _tipoUnidadRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TipoUnidadController(
            IMediator mediator,
            ITipoUnidadRepository tipoUnidadRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _tipoUnidadRepository = tipoUnidadRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Endpoint universal para obtener tipos de unidad con diferentes opciones
        /// </summary>
        /// <param name="id">ID específico del tipo de unidad (opcional)</param>
        /// <param name="page">Número de página para paginación (opcional)</param>
        /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
        /// <param name="search">Término de búsqueda en nombre (opcional)</param>
        /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
        /// <param name="status">Filtrar por status (opcional)</param>
        /// <returns>Lista de tipos de unidad o tipo específico</returns>
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
                // Si se especifica un ID, retornar ese tipo de unidad específico
                if (id.HasValue)
                {
                    var tipoUnidad = await _tipoUnidadRepository.GetByIdAsync(id.Value);
                    if (tipoUnidad == null)
                        return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Tipo de unidad no encontrado" });

                    return Ok(new ApiResponse<object> { Success = true, Data = tipoUnidad });
                }

                // Si es para combo, retornar solo ID y Nombre
                if (combo)
                {
                    var tiposUnidadCombo = await _tipoUnidadRepository.GetAllAsync();
                    var comboData = tiposUnidadCombo
                        .Where(t => !status.HasValue || t.Status == status.Value)
                        .Select(t => new { t.Id, t.Nombre })
                        .ToList();

                    return Ok(new ApiResponse<object> { Success = true, Data = comboData });
                }

                // Obtener todos los tipos de unidad con filtros
                var query = await _tipoUnidadRepository.GetAllAsync();
                var tiposUnidad = query.AsQueryable();

                // Aplicar filtro de búsqueda
                if (!string.IsNullOrEmpty(search))
                {
                    tiposUnidad = tiposUnidad.Where(t => t.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                // Aplicar filtro de status
                if (status.HasValue)
                {
                    tiposUnidad = tiposUnidad.Where(t => t.Status == status.Value);
                }

                // Aplicar paginación si se especifica
                if (page.HasValue)
                {
                    var totalCount = tiposUnidad.Count();
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    var pagedData = tiposUnidad
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
                var allData = tiposUnidad.ToList();
                return Ok(new ApiResponse<object> { Success = true, Data = allData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener tipos de unidad para combobox (solo ID y Nombre)
        /// </summary>
        /// <returns>Lista de tipos de unidad con ID y Nombre</returns>
        [HttpGet("combo")]
        public async Task<ActionResult<ApiResponse<object>>> GetCombo()
        {
            try
            {
                var tiposUnidad = await _tipoUnidadRepository.GetAllAsync();
                var comboData = tiposUnidad
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

        /// <summary>
        /// Crear un nuevo tipo de unidad
        /// </summary>
        /// <param name="tipoUnidad">Datos del tipo de unidad a crear</param>
        /// <returns>Tipo de unidad creado</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateTipoUnidadDto tipoUnidad)
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
                
                var command = new CreateTipoUnidadCommand(tipoUnidad, userId, idEmpresa);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(Get), new { id = result.Id }, new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Tipo de unidad creado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear el tipo de unidad", ex.Message));
            }
        }

        /// <summary>
        /// Actualizar un tipo de unidad existente
        /// </summary>
        /// <param name="id">ID del tipo de unidad</param>
        /// <param name="tipoUnidad">Datos actualizados del tipo de unidad</param>
        /// <returns>Tipo de unidad actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateTipoUnidadDto tipoUnidad)
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
                var command = new UpdateTipoUnidadCommand(id, tipoUnidad, userId);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Tipo de unidad actualizado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar el tipo de unidad", ex.Message));
            }
        }

        /// <summary>
        /// Eliminar un tipo de unidad
        /// </summary>
        /// <param name="id">ID del tipo de unidad a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteTipoUnidadCommand(id);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Tipo de unidad eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar el tipo de unidad", ex.Message));
            }
        }
    }
} 