using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs
{
    public class SucursalDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Responsable { get; set; }
        public bool Activo { get; set; }
        public int? IdEmpresa { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUser { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class CreateSucursalDto
    {
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public bool EsMatriz { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
    }

    public class UpdateSucursalDto
    {
        [Required(ErrorMessage = "The name is required")]
        [StringLength(100, ErrorMessage = "The name cannot exceed 100 characters")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "The description cannot exceed 255 characters")]
        public string? Descripcion { get; set; }

        [Required(ErrorMessage = "The address is required")]
        [StringLength(200, ErrorMessage = "The address cannot exceed 200 characters")]
        public string Direccion { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "The phone number cannot exceed 20 characters")]
        public string? Telefono { get; set; }

        [StringLength(100, ErrorMessage = "The email cannot exceed 100 characters")]
        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "The manager name cannot exceed 100 characters")]
        public string? Responsable { get; set; }
    }

    public class SucursalResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool EsMatriz { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public bool Status { get; set; }
        public int? IdEmpresa { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class CreateSucursalResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool EsMatriz { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public bool Status { get; set; }
        public int? IdEmpresa { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }

    public class UpdateSucursalResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public bool EsMatriz { get; set; }
        public string Direccion { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public bool Status { get; set; }
        public int? IdEmpresa { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class SucursalSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
} 