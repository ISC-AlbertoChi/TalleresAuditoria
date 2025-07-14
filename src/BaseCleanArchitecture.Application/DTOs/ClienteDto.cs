using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs
{
    public class ClienteResponseDto
    {
        public int Id { get; set; }
        public string NombreComercial { get; set; } = string.Empty;
        public string? RazonSocial { get; set; }
        public string? RFC { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? NombreContacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? CorreoContacto { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }

    public class ClienteUpdateResponseDto
    {
        public int Id { get; set; }
        public string NombreComercial { get; set; } = string.Empty;
        public string? RazonSocial { get; set; }
        public string? RFC { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Correo { get; set; }
        public string? NombreContacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? CorreoContacto { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class ClienteDto
    {
        public int Id { get; set; }
        public string NombreComercial { get; set; } = string.Empty;
        public string? RazonSocial { get; set; }
        public string? RFC { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Contacto { get; set; }
        public string? TelefonoContacto { get; set; }
        public string? EmailContacto { get; set; }
        public string? Observaciones { get; set; }
        public bool Activo { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUser { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class CreateClienteDto
    {
        public string NombreComercial { get; set; } = string.Empty;
        public string RazonSocial { get; set; } = string.Empty;
        public string Rfc { get; set; } = string.Empty;
        public string Direccion { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string NombreContacto { get; set; } = string.Empty;
        public string TelefonoContacto { get; set; } = string.Empty;
        public string CorreoContacto { get; set; } = string.Empty;
    }

    public class UpdateClienteDto
    {
        [Required(ErrorMessage = "The commercial name is required")]
        [StringLength(100, ErrorMessage = "The commercial name cannot exceed 100 characters")]
        public string NombreComercial { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "The business name cannot exceed 100 characters")]
        public string? RazonSocial { get; set; }

        [StringLength(13, ErrorMessage = "The RFC cannot exceed 13 characters")]
        public string? RFC { get; set; }

        [StringLength(200, ErrorMessage = "The address cannot exceed 200 characters")]
        public string? Direccion { get; set; }

        [StringLength(20, ErrorMessage = "The phone number cannot exceed 20 characters")]
        public string? Telefono { get; set; }

        [StringLength(100, ErrorMessage = "The email cannot exceed 100 characters")]
        public string? Email { get; set; }

        [StringLength(100, ErrorMessage = "The contact name cannot exceed 100 characters")]
        public string? Contacto { get; set; }

        [StringLength(20, ErrorMessage = "The contact phone cannot exceed 20 characters")]
        public string? TelefonoContacto { get; set; }

        [StringLength(100, ErrorMessage = "The contact email cannot exceed 100 characters")]
        public string? EmailContacto { get; set; }

        [StringLength(500, ErrorMessage = "The observations cannot exceed 500 characters")]
        public string? Observaciones { get; set; }
    }

    public class ClienteSimpleDto
    {
        public int Id { get; set; }
        public string NombreComercial { get; set; } = string.Empty;
    }
} 