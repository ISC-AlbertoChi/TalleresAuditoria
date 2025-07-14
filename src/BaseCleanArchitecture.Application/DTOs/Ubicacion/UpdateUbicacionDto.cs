using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs.Ubicacion;

public class UpdateUbicacionDto
{
    [StringLength(250)]
    public string? Zona { get; set; }

    [StringLength(250)]
    public string? Pasillo { get; set; }

    [StringLength(250)]
    public string? Nivel { get; set; }

    [StringLength(250)]
    public string? Subnivel { get; set; }

    [Required]
    [StringLength(250)]
    public string Clave { get; set; } = null!;

    [StringLength(250)]
    public string? Descripcion { get; set; }

    [Required]
    public int IdSucursal { get; set; }

    [Required]
    public int IdAlmacen { get; set; }
} 