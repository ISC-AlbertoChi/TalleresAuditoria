using AutoMapper;
using BaseCleanArchitecture.Application.Features.Clientes.Commands.CreateCliente;
using BaseCleanArchitecture.Application.Features.Clientes.Commands.UpdateCliente;
using BaseCleanArchitecture.Application.Features.Clientes.Commands.DeleteCliente;
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
    public class ClientesController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IClienteRepository _clienteRepository;
        private readonly IMapper _mapper;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ClientesController(
            IMediator mediator,
            IClienteRepository clienteRepository,
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _clienteRepository = clienteRepository;
            _mapper = mapper;
            _httpContextAccessor = httpContextAccessor;
        }

        /// <summary>
        /// Endpoint universal para obtener clientes con diferentes opciones
        /// </summary>
        /// <param name="id">ID específico del cliente (opcional)</param>
        /// <param name="page">Número de página para paginación (opcional)</param>
        /// <param name="pageSize">Tamaño de página (opcional, default: 10)</param>
        /// <param name="search">Término de búsqueda en nombre (opcional)</param>
        /// <param name="combo">Si es true, retorna solo ID y Nombre para comboboxes (opcional)</param>
        /// <param name="status">Filtrar por status (opcional)</param>
        /// <returns>Lista de clientes o cliente específico</returns>
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
                // Si se especifica un ID, retornar ese cliente específico
                if (id.HasValue)
                {
                    var cliente = await _clienteRepository.GetByIdAsync(id.Value);
                    if (cliente == null)
                        return NotFound(new ApiResponse<object> { Success = false, Mensaje = "Cliente no encontrado" });

                    return Ok(new ApiResponse<object> { Success = true, Data = cliente });
                }

                // Si es para combo, retornar solo ID y Nombre
                if (combo)
                {
                    var clientesCombo = await _clienteRepository.GetAllAsync();
                    var comboData = clientesCombo
                        .Where(c => !status.HasValue || c.Status == status.Value)
                        .Select(c => new { c.Id, c.NombreComercial })
                        .ToList();

                    return Ok(new ApiResponse<object> { Success = true, Data = comboData });
                }

                // Obtener todos los clientes con filtros
                var query = await _clienteRepository.GetAllAsync();
                var clientes = query.AsQueryable();

                // Aplicar filtro de búsqueda
                if (!string.IsNullOrEmpty(search))
                {
                    clientes = clientes.Where(c => c.NombreComercial.Contains(search, StringComparison.OrdinalIgnoreCase));
                }

                // Aplicar filtro de status
                if (status.HasValue)
                {
                    clientes = clientes.Where(c => c.Status == status.Value);
                }

                // Aplicar paginación si se especifica
                if (page.HasValue)
                {
                    var totalCount = clientes.Count();
                    var totalPages = (int)Math.Ceiling((double)totalCount / pageSize);
                    var pagedData = clientes
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
                var allData = clientes.ToList();
                return Ok(new ApiResponse<object> { Success = true, Data = allData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Obtener clientes para combobox (solo ID y Nombre)
        /// </summary>
        /// <returns>Lista de clientes con ID y Nombre</returns>
        [HttpGet("combo")]
        public async Task<ActionResult<ApiResponse<object>>> GetCombo()
        {
            try
            {
                var clientes = await _clienteRepository.GetAllAsync();
                var comboData = clientes
                    .Where(c => c.Status)
                    .Select(c => new { c.Id, c.NombreComercial })
                    .ToList();

                return Ok(new ApiResponse<object> { Success = true, Data = comboData });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ApiResponse<object> { Success = false, Mensaje = $"Error interno del servidor: {ex.Message}" });
            }
        }

        /// <summary>
        /// Crear un nuevo cliente
        /// </summary>
        /// <param name="cliente">Datos del cliente a crear</param>
        /// <returns>Cliente creado</returns>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateClienteDto cliente)
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
                var command = new CreateClienteCommand(cliente, userId);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(Create), new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Cliente creado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear el cliente", ex.Message));
            }
        }

        /// <summary>
        /// Actualizar un cliente existente
        /// </summary>
        /// <param name="id">ID del cliente</param>
        /// <param name="cliente">Datos actualizados del cliente</param>
        /// <returns>Cliente actualizado</returns>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateClienteDto cliente)
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
                var command = new UpdateClienteCommand(id, cliente, userId);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Cliente actualizado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar el cliente", ex.Message));
            }
        }

        /// <summary>
        /// Eliminar un cliente
        /// </summary>
        /// <param name="id">ID del cliente a eliminar</param>
        /// <returns>Resultado de la eliminación</returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var command = new DeleteClienteCommand(id);
                var result = await _mediator.Send(command);
                
                if (!result)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Cliente no encontrado", "El cliente especificado no existe"));
                }

                return Ok(new ApiResponse<object>
                {
                    Success = true,
                    Mensaje = "Cliente eliminado correctamente"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar el cliente", ex.Message));
            }
        }
    }
} 