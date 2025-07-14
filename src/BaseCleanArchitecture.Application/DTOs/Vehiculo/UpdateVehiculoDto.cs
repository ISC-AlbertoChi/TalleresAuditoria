using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs.Vehiculo;

public class UpdateVehiculoDto
{
    [StringLength(250, MinimumLength = 3)]
    public string? NumeroEconomico { get; set; }

    public int? IdMarca { get; set; }

    public int? IdModelo { get; set; }

    [StringLength(250)]
    public string? Placa { get; set; }

    public int? IdPropietario { get; set; }

    [StringLength(250)]
    public string? Serie { get; set; }

    public int? IdTipoCombustible { get; set; }

    [StringLength(255)]
    public string? Observaciones { get; set; }

    public int? IdTipoUnidad { get; set; }

    public int? IdEmpresa { get; set; }
} 