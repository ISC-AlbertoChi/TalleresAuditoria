namespace BaseCleanArchitecture.Application.DTOs.Vehiculo;

public class VehiculoResponseDto
{
    public int Id { get; set; }
    public string NumeroEconomico { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public int? IdMarca { get; set; }
    public int? IdModelo { get; set; }
    public int? IdPropietario { get; set; }
    public int? IdTipoCombustible { get; set; }
    public int? IdTipoUnidad { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? IdUserUpdate { get; set; }
}

public class CreateVehiculoResponseDto
{
    public int Id { get; set; }
    public string NumeroEconomico { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public int? IdMarca { get; set; }
    public int? IdModelo { get; set; }
    public int? IdPropietario { get; set; }
    public int? IdTipoCombustible { get; set; }
    public int? IdTipoUnidad { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
}

public class UpdateVehiculoResponseDto
{
    public int Id { get; set; }
    public string NumeroEconomico { get; set; } = string.Empty;
    public string Placa { get; set; } = string.Empty;
    public string Serie { get; set; } = string.Empty;
    public string? Observaciones { get; set; }
    public int? IdMarca { get; set; }
    public int? IdModelo { get; set; }
    public int? IdPropietario { get; set; }
    public int? IdTipoCombustible { get; set; }
    public int? IdTipoUnidad { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? IdUserUpdate { get; set; }
} 