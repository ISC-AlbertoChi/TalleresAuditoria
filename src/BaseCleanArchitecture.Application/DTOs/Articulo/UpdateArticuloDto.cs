using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs.Articulo;

public class UpdateArticuloDto
{
    [Required]
    [StringLength(255)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    [Required]
    public int IdMarca { get; set; }

    [Required]
    public int IdModelo { get; set; }

    [Required]
    [StringLength(255)]
    public string Material { get; set; } = null!;

    [StringLength(255)]
    public string? Resistencia { get; set; }

    public decimal? Duracion { get; set; }

    public string? Compatibilidad { get; set; }

    public decimal? PrecioUnitario { get; set; }

    public decimal? PrecioVentanilla { get; set; }

    [StringLength(255)]
    public string? CodigoBarras { get; set; }

    [StringLength(255)]
    public string? Serie { get; set; }

    public decimal? PesoPieza { get; set; }

    [StringLength(255)]
    public string? Lote { get; set; }

    public DateTime? FechaLote { get; set; }

    public int? PiezasCaja { get; set; }

    public int? TiempoVida { get; set; }

    [StringLength(255)]
    public string? Factura { get; set; }
} 