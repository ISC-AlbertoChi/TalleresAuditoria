using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs.Articulo;

public class CreateArticuloDto
{
    [Required(ErrorMessage = "El nombre es obligatorio")]
    [StringLength(255, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 255 caracteres")]
    public string Nombre { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "La descripción no puede exceder los 255 caracteres")]
    public string Descripcion { get; set; } = string.Empty;

    public int IdMarca { get; set; }

    public int IdModelo { get; set; }

    [StringLength(255, ErrorMessage = "El material no puede exceder los 255 caracteres")]
    public string Material { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "La resistencia no puede exceder los 255 caracteres")]
    public string Resistencia { get; set; } = string.Empty;

    public decimal Duracion { get; set; }

    [StringLength(255, ErrorMessage = "La compatibilidad no puede exceder los 255 caracteres")]
    public string Compatibilidad { get; set; } = string.Empty;

    public decimal PrecioUnitario { get; set; }

    public decimal PrecioVentanilla { get; set; }

    [StringLength(255, ErrorMessage = "El código de barras no puede exceder los 255 caracteres")]
    public string CodigoBarras { get; set; } = string.Empty;

    [StringLength(255, ErrorMessage = "La serie no puede exceder los 255 caracteres")]
    public string Serie { get; set; } = string.Empty;

    public decimal PesoPieza { get; set; }

    [StringLength(255, ErrorMessage = "El lote no puede exceder los 255 caracteres")]
    public string Lote { get; set; } = string.Empty;

    public DateTime FechaLote { get; set; }

    public int PiezasCaja { get; set; }

    public int TiempoVida { get; set; }

    [StringLength(255, ErrorMessage = "La factura no puede exceder los 255 caracteres")]
    public string Factura { get; set; } = string.Empty;
} 