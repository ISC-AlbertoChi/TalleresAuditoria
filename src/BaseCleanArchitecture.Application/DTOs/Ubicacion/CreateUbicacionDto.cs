using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs.Ubicacion;

public class CreateUbicacionDto
{
    public string Clave { get; set; } = string.Empty;
    public string Zona { get; set; } = string.Empty;
    public string Pasillo { get; set; } = string.Empty;
    public string Nivel { get; set; } = string.Empty;
    public string Subnivel { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int IdSucursal { get; set; }
    public int IdAlmacen { get; set; }
} 