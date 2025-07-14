using AutoMapper;
using BaseCleanArchitecture.Application.Features.Vehiculos.Commands.CreateVehiculo;
using BaseCleanArchitecture.Application.Features.Vehiculos.Commands.UpdateVehiculo;
using BaseCleanArchitecture.Application.Features.Vehiculos.Commands.DeleteVehiculo;
using BaseCleanArchitecture.Application.DTOs.Vehiculo;
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
    public class VehiculoController : ControllerBase
    {
        private readonly IMediator _mediator;
        private readonly IVehiculoRepository _vehiculoRepository;
        private readonly IMapper _mapper;

        public VehiculoController(
            IMediator mediator,
            IVehiculoRepository vehiculoRepository,
            IMapper mapper)
        {
            _mediator = mediator;
            _vehiculoRepository = vehiculoRepository;
            _mapper = mapper;
        }

        /// <summary>
        /// GET Universal - Sirve para todo: obtener por ID, lista, combobox, búsqueda y filtros
        /// </summary>
        /// <param name="id">ID específico del vehículo</param>
        /// <param name="list">true para obtener lista completa</param>
        /// <param name="combo">true para obtener solo ID y Nombre (combobox)</param>
        /// <param name="search">Texto para búsqueda</param>
        /// <param name="filter">Campo específico para filtrar</param>
        /// <param name="page">Número de página</param>
        /// <param name="size">Tamaño de página</param>
        [HttpGet]
        public async Task<IActionResult> Get(
            [FromQuery] int? id = null,
            [FromQuery] bool list = false,
            [FromQuery] bool combo = false,
            [FromQuery] string? search = null,
            [FromQuery] string? filter = null,
            [FromQuery] int page = 1,
            [FromQuery] int size = 10)
        {
            try
            {
                // Si se proporciona ID, obtener vehículo específico
                if (id.HasValue)
                {
                    var vehiculo = await _vehiculoRepository.GetByIdAsync(id.Value);
                    if (vehiculo == null)
                    {
                        return NotFound(ApiResponse<object>.ErrorResponse("Vehículo no encontrado", "El vehículo especificado no existe"));
                    }

                    var vehiculoDto = _mapper.Map<VehiculoResponseDto>(vehiculo);
                    return Ok(new ApiResponse<VehiculoResponseDto>
                    {
                        Success = true,
                        Mensaje = "Vehículo obtenido correctamente",
                        Data = vehiculoDto
                    });
                }

                // Obtener todos los vehículos
                var vehiculos = await _vehiculoRepository.GetAllAsync();

                // Aplicar filtros
                if (!string.IsNullOrEmpty(search))
                {
                    vehiculos = vehiculos.Where(v => 
                        v.NumeroEconomico.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        v.Placa.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        v.Serie.Contains(search, StringComparison.OrdinalIgnoreCase) ||
                        (v.Observaciones != null && v.Observaciones.Contains(search, StringComparison.OrdinalIgnoreCase))
                    ).ToList();
                }

                // Filtrar solo activos si no se especifica list
                if (!list)
                {
                    vehiculos = vehiculos.Where(v => v.Status).ToList();
                }

                // Aplicar filtro específico
                if (!string.IsNullOrEmpty(filter))
                {
                    // Aquí puedes agregar lógica de filtro específico según el campo
                    // Por ejemplo: filter=propietario:3 para filtrar por propietario
                }

                // Aplicar paginación
                var totalCount = vehiculos.Count;
                var totalPages = (int)Math.Ceiling((double)totalCount / size);
                var pagedVehiculos = vehiculos
                    .Skip((page - 1) * size)
                    .Take(size)
                    .ToList();

                // Mapear según el tipo de respuesta
                if (combo)
                {
                    var comboVehiculos = pagedVehiculos.Select(v => new
                    {
                        Id = v.Id,
                        Nombre = $"{v.NumeroEconomico} - {v.Placa}"
                    }).ToList();

                    return Ok(new ApiResponse<object>
                    {
                        Success = true,
                        Mensaje = $"Se obtuvieron {comboVehiculos.Count} vehículos para combobox",
                        Data = new
                        {
                            Items = comboVehiculos,
                            TotalCount = totalCount,
                            TotalPages = totalPages,
                            CurrentPage = page,
                            PageSize = size
                        }
                    });
                }
                else
                {
                    var vehiculosDto = _mapper.Map<IEnumerable<VehiculoResponseDto>>(pagedVehiculos);
                    return Ok(new ApiResponse<object>
                    {
                        Success = true,
                        Mensaje = $"Se obtuvieron {vehiculosDto.Count()} vehículos",
                        Data = new
                        {
                            Items = vehiculosDto,
                            TotalCount = totalCount,
                            TotalPages = totalPages,
                            CurrentPage = page,
                            PageSize = size
                        }
                    });
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al obtener vehículos", ex.Message));
            }
        }

        /// <summary>
        /// Crea un nuevo vehículo
        /// </summary>
        [HttpPost]
        public async Task<IActionResult> Create([FromBody] CreateVehiculoDto vehiculo)
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
                
                var command = new CreateVehiculoCommand(vehiculo, userId, idEmpresa);
                var result = await _mediator.Send(command);
                
                return CreatedAtAction(nameof(Create), new ApiResponse<CreateVehiculoResponseDto>
                {
                    Success = true,
                    Mensaje = "Vehículo creado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al crear el vehículo", ex.Message));
            }
        }

        /// <summary>
        /// Actualiza un vehículo existente
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] UpdateVehiculoDto vehiculo)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ApiResponse<object>.ErrorResponse("Datos de entrada inválidos", 
                        ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList()));
                }

                var existingVehiculo = await _vehiculoRepository.GetByIdAsync(id);
                if (existingVehiculo == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Vehículo no encontrado", "El vehículo especificado no existe"));
                }

                var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier);
                if (userIdClaim == null)
                {
                    return Unauthorized(ApiResponse<object>.ErrorResponse("Usuario no autenticado", "Usuario no autenticado"));
                }

                var userId = int.Parse(userIdClaim.Value);
                var command = new UpdateVehiculoCommand(id, vehiculo, userId);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<UpdateVehiculoResponseDto>
                {
                    Success = true,
                    Mensaje = "Vehículo actualizado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al actualizar el vehículo", ex.Message));
            }
        }

        /// <summary>
        /// Elimina un vehículo
        /// </summary>
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var existingVehiculo = await _vehiculoRepository.GetByIdAsync(id);
                if (existingVehiculo == null)
                {
                    return NotFound(ApiResponse<object>.ErrorResponse("Vehículo no encontrado", "El vehículo especificado no existe"));
                }

                var command = new DeleteVehiculoCommand(id);
                var result = await _mediator.Send(command);
                
                return Ok(new ApiResponse<bool>
                {
                    Success = true,
                    Mensaje = "Vehículo eliminado correctamente",
                    Data = result
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, ApiResponse<object>.ErrorResponse("Error al eliminar el vehículo", ex.Message));
            }
        }
    }
} 