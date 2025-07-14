namespace BaseCleanArchitecture.Application.DTOs;

public class TipoCombustibleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
    public string? Descripcion { get; set; }
    public bool Activo { get; set; }
}

public class TipoCombustibleSimpleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
} 