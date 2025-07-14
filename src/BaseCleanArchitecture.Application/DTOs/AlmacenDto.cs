using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs;

public class AlmacenDto
{
    public int Id { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int IdSucursal { get; set; }
    public int? IdEmpresa { get; set; }
    public bool Activo { get; set; }
    public DateTime? DateCreate { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? IdUser { get; set; }
    public int? IdUserUpdate { get; set; }
}

public class AlmacenResponseDto
{
    public int Id { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int IdSucursal { get; set; }
    public int? IdEmpresa { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? IdUserUpdate { get; set; }
}

public class CreateAlmacenResponseDto
{
    public int Id { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int IdSucursal { get; set; }
    public int? IdEmpresa { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
}

public class UpdateAlmacenResponseDto
{
    public int Id { get; set; }
    public string Clave { get; set; } = string.Empty;
    public string? Nombre { get; set; }
    public string Descripcion { get; set; } = string.Empty;
    public int IdSucursal { get; set; }
    public int? IdEmpresa { get; set; }
    public bool Status { get; set; }
    public DateTime? DateCreate { get; set; }
    public int? IdUser { get; set; }
    public DateTime? DateUpdate { get; set; }
    public int? IdUserUpdate { get; set; }
}

public class CreateAlmacenDto
{
    public string Clave { get; set; } = string.Empty;
    public string Nombre { get; set; } = string.Empty;
    public string Descripcion { get; set; } = string.Empty;
    public int IdSucursal { get; set; }
}

public class UpdateAlmacenDto
{
    [Required(ErrorMessage = "The key is required")]
    [StringLength(250, MinimumLength = 3, ErrorMessage = "The key must be between 3 and 250 characters")]
    public string Clave { get; set; } = string.Empty;

    [StringLength(250, MinimumLength = 3, ErrorMessage = "The name must be between 3 and 250 characters")]
    public string? Nombre { get; set; }

    [Required(ErrorMessage = "The description is required")]
    [StringLength(255, ErrorMessage = "The description cannot exceed 255 characters")]
    public string Descripcion { get; set; } = string.Empty;

    [Required(ErrorMessage = "The branch is required")]
    public int IdSucursal { get; set; }
}

public class AlmacenSimpleDto
{
    public int Id { get; set; }
    public string Nombre { get; set; } = string.Empty;
} 