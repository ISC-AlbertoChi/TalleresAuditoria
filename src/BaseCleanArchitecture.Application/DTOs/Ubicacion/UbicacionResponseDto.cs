namespace BaseCleanArchitecture.Application.DTOs.Ubicacion;

public class UbicacionResponseDto
{
    public int Id { get; set; }
    public string? Zona { get; set; }
    public string? Pasillo { get; set; }
    public string? Nivel { get; set; }
    public string? Subnivel { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int IdSucursal { get; set; }
    public int IdAlmacen { get; set; }
    public int? IdEmpresa { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? IdUserUpdate { get; set; }
}

public class CreateUbicacionResponseDto
{
    public int Id { get; set; }
    public string? Zona { get; set; }
    public string? Pasillo { get; set; }
    public string? Nivel { get; set; }
    public string? Subnivel { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int IdSucursal { get; set; }
    public int IdAlmacen { get; set; }
    public int? IdEmpresa { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
}

public class UpdateUbicacionResponseDto
{
    public int Id { get; set; }
    public string? Zona { get; set; }
    public string? Pasillo { get; set; }
    public string? Nivel { get; set; }
    public string? Subnivel { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int IdSucursal { get; set; }
    public int IdAlmacen { get; set; }
    public int? IdEmpresa { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? IdUserUpdate { get; set; }
} 