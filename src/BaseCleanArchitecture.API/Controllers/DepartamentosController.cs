using AutoMapper;
using BaseCleanArchitecture.Application.Features.Departamentos.Commands.CreateDepartamento;
using BaseCleanArchitecture.Application.Features.Departamentos.Commands.UpdateDepartamento;
using BaseCleanArchitecture.Application.Features.Departamentos.Commands.DeleteDepartamento;
using BaseCleanArchitecture.Application.DTOs;
using BaseCleanArchitecture.Domain.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;

namespace BaseCleanArchitecture.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class DepartamentosController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IDepartamentoRepository _departamentoRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public DepartamentosController(
            IMediator mediator,
            IDepartamentoRepository departamentoRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _departamentoRepository = departamentoRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Endpoint universal para obtener departamentos con diferentes opciones
        /// </summary>
        /// <param name="id">ID específico del departamento (opcional)</param>
        /// <param name="page">Número de página para paginación (opcional)</param>
        /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
        /// <param name="search">Término de búsqueda en nombre (opcional)</param>
        /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
        /// <param name="status">Filtrar por status (opcional)</param>
        /// <returns>Lista de departamentos o departamento específico</returns>
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
                // Si se especifica un ID, retornar ese departamento específico
                if (id.HasValue)
                {
                    var departamento = await _departamentoRepository.GetByIdAsync(id.Value);
                    if (departamento == null)
                        return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Departamento no encontrado" });

                    return Ok(new ApiResponse<object> { Success = true, Data = departamento });
                }

                // Si es para combo, retornar solo ID y Nombre
                if (combo)
                {
                    var departamentosCombo = await _departamentoRepository.GetAllAsync();
                    var comboData = departamentosCombo
                        .Where(d => !status.HasValue || d.Status == status.Value)
                        .Select(d => new { d.Id, d.Nombre })
                        .ToList();

                    return Ok(new ApiResponse<object> { Success = true, Data = comboData });
                }

                // Obtener todos los departamentos con filtros
                var query = await _departamentoRepository.GetAllAsync();
                var departamentos = query.AsQueryable();

                // Aplicar filtro de búsqueda
                if (!string.IsNullOrEmpty(search))
                {
                    departamentos = departamentos.Where(d => d.Nombre.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                // Aplicar filtro de status
                if (status.HasValue)
                {
                    departamentos = departamentos.Where(d => d.Status == status.Value);
                }

                // Aplicar paginación si se especifica
                if (page.HasValue)
                {
                    var totalCount = departamentos.Count();
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    var pagedData = departamentos
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
                var allData = departamentos.ToList();
                return Ok(new ApiResponse<object> { Success = true, Data = allData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Crear un nuevo departamento
        /// </summary>
        /// <param name="departamento">Datos del departamento a crear</param>
        /// <returns>Departamento creado</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateDepartamentoDto departamento)
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
                
                var command = new CreateDepartamentoCommand(departamento, userId, idEmpresa);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(Get), new { id = result.Id }, new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Departamento creado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear el departamento", ex.Message));
            }
        }

        /// <summary>
        /// Actualizar un departamento existente
        /// </summary>
        /// <param name="id">ID del departamento</param>
        /// <param name="departamento">Datos actualizados del departamento</param>
        /// <returns>Departamento actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateDepartamentoDto departamento)
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
                var command = new UpdateDepartamentoCommand(id, departamento, userId);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Departamento actualizado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar el departamento", ex.Message));
            }
        }

        /// <summary>
        /// Eliminar un departamento
        /// </summary>
        /// <param name="id">ID del departamento a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteDepartamentoCommand(id);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Departamento eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar el departamento", ex.Message));
            }
        }
    }
} 