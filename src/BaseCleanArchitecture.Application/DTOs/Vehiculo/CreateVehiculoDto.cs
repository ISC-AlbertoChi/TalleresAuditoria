using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs.Vehiculo;

public class CreateVehiculoDto
{
    public string NumeroEconomico { get; set; } = string.Empty;
    public int? IdMarca { get; set; }
    public int? IdModelo { get; set; }
    public string Placa { get; set; } = string.Empty;
    public int IdPropietario { get; set; }
    public string Serie { get; set; } = string.Empty;
    public int? IdTipoCombustible { get; set; }
    public string? Observaciones { get; set; }
    public int? IdTipoUnidad { get; set; }
} 