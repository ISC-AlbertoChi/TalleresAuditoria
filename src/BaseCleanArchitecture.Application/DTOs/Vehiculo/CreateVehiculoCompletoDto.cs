using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs.Vehiculo;

public class CreateVehiculoCompletoDto
{
    [Required]
    [StringLength(250, MinimumLength = 3)]
    public string NumeroEconomico { get; set; } = null!;

    // Campos para crear la marca
    [Required]
    [StringLength(255)]
    public string NombreMarca { get; set; } = null!;

    [StringLength(255)]
    public string? DescripcionMarca { get; set; }

    // Campos para crear el modelo
    [Required]
    [StringLength(100)]
    public string NombreModelo { get; set; } = null!;

    [StringLength(255)]
    public string? DescripcionModelo { get; set; }

    [Required]
    [StringLength(250)]
    public string Placa { get; set; } = null!;

    [Required]
    public int IdPropietario { get; set; }

    [Required]
    [StringLength(250)]
    public string Serie { get; set; } = null!;

    public int? IdTipoCombustible { get; set; }

    [StringLength(255)]
    public string? Observaciones { get; set; }

    public int? IdTipoUnidad { get; set; }
} 