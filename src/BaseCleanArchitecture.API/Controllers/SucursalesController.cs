using AutoMapper;
using BaseCleanArchitecture.Application.Features.Sucursales.Commands.CreateSucursal;
using BaseCleanArchitecture.Application.Features.Sucursales.Commands.UpdateSucursal;
using BaseCleanArchitecture.Application.Features.Sucursales.Commands.DeleteSucursal;
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
    public class SucursalesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly ISucursalRepository _sucursalRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public SucursalesController(
            IMediator mediator,
            ISucursalRepository sucursalRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _sucursalRepository = sucursalRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Endpoint universal para obtener sucursales con diferentes opciones
        /// </summary>
        /// <param name="id">ID específico de la sucursal (opcional)</param>
        /// <param name="page">Número de página para paginación (opcional)</param>
        /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
        /// <param name="search">Término de búsqueda en nombre (opcional)</param>
        /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
        /// <param name="status">Filtrar por status (opcional)</param>
        /// <returns>Lista de sucursales o sucursal específica</returns>
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
                // Si se especifica un ID, retornar esa sucursal específica
                if (id.HasValue)
                {
                    var sucursal = await _sucursalRepository.GetByIdAsync(id.Value);
                    if (sucursal == null)
                        return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Sucursal no encontrada" });

                    return Ok(new ApiResponse<object> { Success = true, Data = sucursal });
                }

                // Si es para combo, retornar solo ID y Nombre
                if (combo)
                {
                    var sucursalesCombo = await _sucursalRepository.GetAllAsync();
                    var comboData = sucursalesCombo
                        .Where(s => !status.HasValue || s.Status == status.Value)
                        .Select(s => new { s.Id, s.Nombre })
                        .ToList();

                    return Ok(new ApiResponse<object> { Success = true, Data = comboData });
                }

                // Obtener todas las sucursales con filtros
                var query = await _sucursalRepository.GetAllAsync();
                var sucursales = query.AsQueryable();

                // Aplicar filtro de búsqueda
                if (!string.IsNullOrEmpty(search))
                {
                    sucursales = sucursales.Where(s => s.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                // Aplicar filtro de status
                if (status.HasValue)
                {
                    sucursales = sucursales.Where(s => s.Status == status.Value);
                }

                // Aplicar paginación si se especifica
                if (page.HasValue)
                {
                    var totalCount = sucursales.Count();
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    var pagedData = sucursales
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
                var allData = sucursales.ToList();
                return Ok(new ApiResponse<object> { Success = true, Data = allData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener sucursales para combobox (solo ID y Nombre)
        /// </summary>
        /// <returns>Lista de sucursales con ID y Nombre</returns>
        [HttpGet("combo")]
        public async Task<ActionResult<ApiResponse<object>>> GetCombo()
        {
            try
            {
                var sucursales = await _sucursalRepository.GetAllAsync();
                var comboData = sucursales
                    .Where(s => s.Status)
                    .Select(s => new { s.Id, s.Nombre })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Crear una nueva sucursal
        /// </summary>
        /// <param name="sucursal">Datos de la sucursal a crear</param>
        /// <returns>Sucursal creada</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateSucursalDto sucursal)
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
                var command = new CreateSucursalCommand(sucursal, userId, idEmpresa);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(Create), new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Sucursal creada correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear la sucursal", ex.Message));
            }
        }

        /// <summary>
        /// Actualizar una sucursal existente
        /// </summary>
        /// <param name="id">ID de la sucursal</param>
        /// <param name="sucursal">Datos actualizados de la sucursal</param>
        /// <returns>Sucursal actualizada</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateSucursalDto sucursal)
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
                var command = new UpdateSucursalCommand(id, sucursal, userId);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Sucursal actualizada correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar la sucursal", ex.Message));
            }
        }

        /// <summary>
        /// Eliminar una sucursal
        /// </summary>
        /// <param name="id">ID de la sucursal a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteSucursalCommand(id);
                var result = await _mediator.Send(command);
                
                if (!result)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Sucursal no encontrada", "La sucursal especificada no existe"));
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Sucursal eliminada correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar la sucursal", ex.Message));
            }
        }
    }
} 