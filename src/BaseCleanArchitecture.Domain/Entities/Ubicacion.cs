using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCleanArchitecture.Domain.Entities;

public class Ubicacion
{
    [Key]
    public int Id { get; set; }

    [StringLength(250, MinimumLength = 3)]
    public string? Zona { get; set; }

    [StringLength(250, MinimumLength = 3)]
    public string? Pasillo { get; set; }

    [StringLength(250)]
    public string? Nivel { get; set; }

    [StringLength(250)]
    public string? Subnivel { get; set; }

    [Required]
    [StringLength(250, MinimumLength = 3)]
    public string Clave { get; set; } = null!;

    [StringLength(250)]
    public string? Descripcion { get; set; }

    [Required]
    public int IdSucursal { get; set; }

    [Required]
    public int IdAlmacen { get; set; }

    public int? IdEmpresa { get; set; }

    public bool Status { get; set; } = true;

    public DateTime? DateCreate { get; set; }

    public int? IdUser { get; set; }

    public DateTime? DateUpdate { get; set; }

    public int? IdUserUpdate { get; set; }

    // Relaciones
    [ForeignKey("IdSucursal")]
    public virtual Sucursal Sucursal { get; set; } = null!;

    [ForeignKey("IdAlmacen")]
    public virtual Almacen Almacen { get; set; } = null!;

    // Empresa se maneja como entidad de apoyo

    [ForeignKey("IdUser")]
    public virtual Usuario? Usuario { get; set; }

    [ForeignKey("IdUserUpdate")]
    public virtual Usuario? UsuarioUpdate { get; set; }
} 