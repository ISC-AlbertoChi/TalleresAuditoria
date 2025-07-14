using AutoMapper;
using BaseCleanArchitecture.Application.Features.Marcas.Commands.CreateMarca;
using BaseCleanArchitecture.Application.Features.Marcas.Commands.UpdateMarca;
using BaseCleanArchitecture.Application.Features.Marcas.Commands.DeleteMarca;
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
    public class MarcasController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IMarcaRepository _marcaRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MarcasController(
            IMediator mediator,
            IMarcaRepository marcaRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _marcaRepository = marcaRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Endpoint universal para obtener marcas con diferentes opciones
        /// </summary>
        /// <param name="id">ID específico de la marca (opcional)</param>
        /// <param name="page">Número de página para paginación (opcional)</param>
        /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
        /// <param name="search">Término de búsqueda en nombre (opcional)</param>
        /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
        /// <param name="status">Filtrar por status (opcional)</param>
        /// <returns>Lista de marcas o marca específica</returns>
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
                // Si se especifica un ID, retornar esa marca específica
                if (id.HasValue)
                {
                    var marca = await _marcaRepository.GetByIdAsync(id.Value);
                    if (marca == null)
                        return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Marca no encontrada" });

                    return Ok(new ApiResponse<object> { Success = true, Data = marca });
                }

                // Si es para combo, retornar solo ID y Nombre
                if (combo)
                {
                    var marcasCombo = await _marcaRepository.GetAllAsync();
                    var comboData = marcasCombo
                        .Where(m => !status.HasValue || m.Status == status.Value)
                        .Select(m => new { m.Id, m.Nombre })
                        .ToList();

                    return Ok(new ApiResponse<object> { Success = true, Data = comboData });
                }

                // Obtener todas las marcas con filtros
                var query = await _marcaRepository.GetAllAsync();
                var marcas = query.AsQueryable();

                // Aplicar filtro de búsqueda
                if (!string.IsNullOrEmpty(search))
                {
                    marcas = marcas.Where(m => m.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                // Aplicar filtro de status
                if (status.HasValue)
                {
                    marcas = marcas.Where(m => m.Status == status.Value);
                }

                // Aplicar paginación si se especifica
                if (page.HasValue)
                {
                    var totalCount = marcas.Count();
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    var pagedData = marcas
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
                var allData = marcas.ToList();
                return Ok(new ApiResponse<object> { Success = true, Data = allData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener marcas para combobox (solo ID y Nombre)
        /// </summary>
        /// <returns>Lista de marcas con ID y Nombre</returns>
        [HttpGet("combo")]
        public async Task<ActionResult<ApiResponse<object>>> GetCombo()
        {
            try
            {
                var marcas = await _marcaRepository.GetAllAsync();
                var comboData = marcas
                    .Where(m => m.Status)
                    .Select(m => new { m.Id, m.Nombre })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Crear una nueva marca
        /// </summary>
        /// <param name="marca">Datos de la marca a crear</param>
        /// <returns>Marca creada</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateMarcaDto marca)
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
                
                var command = new CreateMarcaCommand(marca, userId, idEmpresa);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(Create), new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Marca creada correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear la marca", ex.Message));
            }
        }

        /// <summary>
        /// Actualizar una marca existente
        /// </summary>
        /// <param name="id">ID de la marca</param>
        /// <param name="marca">Datos actualizados de la marca</param>
        /// <returns>Marca actualizada</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateMarcaDto marca)
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
                var command = new UpdateMarcaCommand(id, marca, userId);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Marca actualizada correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar la marca", ex.Message));
            }
        }

        /// <summary>
        /// Eliminar una marca
        /// </summary>
        /// <param name="id">ID de la marca a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteMarcaCommand(id);
                var result = await _mediator.Send(command);
                
                if (!result)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Marca no encontrada", "La marca especificada no existe"));
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Marca eliminada correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar la marca", ex.Message));
            }
        }
    }
} 