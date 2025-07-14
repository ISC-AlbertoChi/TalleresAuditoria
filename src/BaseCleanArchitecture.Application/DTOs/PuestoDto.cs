namespace BaseCleanArchitecture.Application.DTOs;

public class PuestoDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int IdEmpresa { get; set; }
    public bool Activo { get; set; }
}

public class PuestoSimpleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
} 