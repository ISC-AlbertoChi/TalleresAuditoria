namespace BaseCleanArchitecture.Application.DTOs;

public class RolDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public int? IdEmpresa { get; set; }
    public bool Activo { get; set; }
}

public class RolSimpleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
} 