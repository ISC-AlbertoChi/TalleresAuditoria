namespace BaseCleanArchitecture.Application.DTOs.Articulo;

public class ArticuloDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? IdMarca { get; set; }
    public int? IdModelo { get; set; }
    public string Material { get; set; } = string.Empty;
    public string? Resistencia { get; set; }
    public decimal Duracion { get; set; }
    public string? Compatibilidad { get; set; }
    public decimal? PrecioUnitario { get; set; }
    public decimal? PrecioVentanilla { get; set; }
    public string? CodigoBarras { get; set; }
    public string? Serie { get; set; }
    public decimal? PesoPieza { get; set; }
    public string? Lote { get; set; }
    public DateTime? FechaLote { get; set; }
    public int? PiezasCaja { get; set; }
    public int? TiempoVida { get; set; }
    public string? Factura { get; set; }
    public bool Activo { get; set; }
    public DateTime? DateCreate { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? IdUser { get; set; }
    public int? IdUserUpdate { get; set; }
}

public class ArticuloSimpleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
} 