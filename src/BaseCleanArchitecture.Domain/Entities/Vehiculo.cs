using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCleanArchitecture.Domain.Entities;

public class Vehiculo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(250, MinimumLength = 3)]
    public string NumeroEconomico { get; set; } = null!;

    public int? IdMarca { get; set; }

    public int? IdModelo { get; set; }

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

    public int? IdEmpresa { get; set; }

    public bool Status { get; set; } = true;

    public DateTime? DateCreate { get; set; }

    public int? IdUser { get; set; }

    public DateTime? DateUpdate { get; set; }

    public int? IdUserUpdate { get; set; }

    // Relaciones
    [ForeignKey("IdMarca")]
    public virtual Marca? Marca { get; set; }

    // Modelo se maneja como entidad de apoyo

    [ForeignKey("IdPropietario")]
    public virtual Cliente Propietario { get; set; } = null!;

    // TipoCombustible se maneja como entidad de apoyo

    [ForeignKey("IdTipoUnidad")]
    public virtual TipoUnidad? TipoUnidad { get; set; }

    // Empresa se maneja como entidad de apoyo

    [ForeignKey("IdUser")]
    public virtual Usuario? Usuario { get; set; }

    [ForeignKey("IdUserUpdate")]
    public virtual Usuario? UsuarioUpdate { get; set; }
} 