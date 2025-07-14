using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs;

public class ModeloDto
{
    public int Id { get; set; }
    public string NombreModelo { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int IdMarca { get; set; }
    public string MarcaNombre { get; set; } = string.Empty;
    public int? AnoInicio { get; set; }
    public int? AnoFin { get; set; }
    public string? Motor { get; set; }
    public string? Transmision { get; set; }
    public string? Combustible { get; set; }
    public decimal? Cilindrada { get; set; }
    public int? Potencia { get; set; }
    public int? Torque { get; set; }
    public string? Carroceria { get; set; }
    public int? Puertas { get; set; }
    public int? Asientos { get; set; }
    public decimal? Peso { get; set; }
    public decimal? Largo { get; set; }
    public decimal? Ancho { get; set; }
    public decimal? Alto { get; set; }
    public decimal? DistanciaEjes { get; set; }
    public decimal? CapacidadTanque { get; set; }
    public decimal? ConsumoUrbano { get; set; }
    public decimal? ConsumoCarretera { get; set; }
    public decimal? ConsumoMixto { get; set; }
    public bool Activo { get; set; }
    public DateTime DateCreate { get; set; }
    public DateTime? DateUpdate { get; set; }
}

public class CreateModeloDto
{
    [Required(ErrorMessage = "The model name is required")]
    [StringLength(100, ErrorMessage = "The model name cannot exceed 100 characters")]
    public string NombreModelo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "The description cannot exceed 500 characters")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "The brand is required")]
    public int IdMarca { get; set; }

    public int? AnoInicio { get; set; }
    public int? AnoFin { get; set; }

    [StringLength(50, ErrorMessage = "The engine cannot exceed 50 characters")]
    public string? Motor { get; set; }

    [StringLength(50, ErrorMessage = "The transmission cannot exceed 50 characters")]
    public string? Transmision { get; set; }

    [StringLength(30, ErrorMessage = "The fuel type cannot exceed 30 characters")]
    public string? Combustible { get; set; }

    public decimal? Cilindrada { get; set; }
    public int? Potencia { get; set; }
    public int? Torque { get; set; }

    [StringLength(30, ErrorMessage = "The body type cannot exceed 30 characters")]
    public string? Carroceria { get; set; }

    public int? Puertas { get; set; }
    public int? Asientos { get; set; }
    public decimal? Peso { get; set; }
    public decimal? Largo { get; set; }
    public decimal? Ancho { get; set; }
    public decimal? Alto { get; set; }
    public decimal? DistanciaEjes { get; set; }
    public decimal? CapacidadTanque { get; set; }
    public decimal? ConsumoUrbano { get; set; }
    public decimal? ConsumoCarretera { get; set; }
    public decimal? ConsumoMixto { get; set; }
}

public class UpdateModeloDto
{
    [Required(ErrorMessage = "The model name is required")]
    [StringLength(100, ErrorMessage = "The model name cannot exceed 100 characters")]
    public string NombreModelo { get; set; } = string.Empty;

    [StringLength(500, ErrorMessage = "The description cannot exceed 500 characters")]
    public string? Descripcion { get; set; }

    [Required(ErrorMessage = "The brand is required")]
    public int IdMarca { get; set; }

    public int? AnoInicio { get; set; }
    public int? AnoFin { get; set; }

    [StringLength(50, ErrorMessage = "The engine cannot exceed 50 characters")]
    public string? Motor { get; set; }

    [StringLength(50, ErrorMessage = "The transmission cannot exceed 50 characters")]
    public string? Transmision { get; set; }

    [StringLength(30, ErrorMessage = "The fuel type cannot exceed 30 characters")]
    public string? Combustible { get; set; }

    public decimal? Cilindrada { get; set; }
    public int? Potencia { get; set; }
    public int? Torque { get; set; }

    [StringLength(30, ErrorMessage = "The body type cannot exceed 30 characters")]
    public string? Carroceria { get; set; }

    public int? Puertas { get; set; }
    public int? Asientos { get; set; }
    public decimal? Peso { get; set; }
    public decimal? Largo { get; set; }
    public decimal? Ancho { get; set; }
    public decimal? Alto { get; set; }
    public decimal? DistanciaEjes { get; set; }
    public decimal? CapacidadTanque { get; set; }
    public decimal? ConsumoUrbano { get; set; }
    public decimal? ConsumoCarretera { get; set; }
    public decimal? ConsumoMixto { get; set; }
}

public class ModeloResponseDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
    public DateTime FechaCreacion { get; set; }
    public DateTime? FechaModificacion { get; set; }
}

public class ModeloSimpleDto
{
    public int Id { get; set; }
    public string NombreModelo { get; set; } = string.Empty;
} 