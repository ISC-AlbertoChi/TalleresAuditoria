using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCleanArchitecture.Domain.Entities;

public class Articulo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 3)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    public int? IdMarca { get; set; }

    public int? IdModelo { get; set; }

    [Required]
    [StringLength(255, MinimumLength = 2)]
    public string Material { get; set; } = null!;

    [StringLength(255)]
    public string? Resistencia { get; set; }

    [Required]
    [Range(1, double.MaxValue)]
    [Column(TypeName = "decimal(5,2)")]
    public decimal Duracion { get; set; }

    public string? Compatibilidad { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? PrecioUnitario { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? PrecioVentanilla { get; set; }

    [StringLength(255)]
    public string? CodigoBarras { get; set; }

    [StringLength(255)]
    public string? Serie { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal? PesoPieza { get; set; }

    [StringLength(255)]
    public string? Lote { get; set; }

    public DateTime? FechaLote { get; set; }

    public int? PiezasCaja { get; set; }

    public int? TiempoVida { get; set; }

    [StringLength(255)]
    public string? Factura { get; set; }

    public bool Status { get; set; } = true;

    public DateTime? DateCreate { get; set; }

    public int? IdUser { get; set; }

    public DateTime? DateUpdate { get; set; }

    public int? IdUserUpdate { get; set; }

    // Relaciones
    [ForeignKey("IdMarca")]
    public virtual Marca? Marca { get; set; }

    // Modelo se maneja como entidad de apoyo

    [ForeignKey("IdUser")]
    public virtual Usuario? Usuario { get; set; }

    [ForeignKey("IdUserUpdate")]
    public virtual Usuario? UsuarioUpdate { get; set; }
} 