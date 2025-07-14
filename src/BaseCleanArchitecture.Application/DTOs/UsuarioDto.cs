using System;
using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public int IdRol { get; set; }
        public string RolNombre { get; set; } = string.Empty;
        public int IdDepartamento { get; set; }
        public string DepartamentoNombre { get; set; } = string.Empty;
        public int IdPuesto { get; set; }
        public string PuestoNombre { get; set; } = string.Empty;
        public int IdSucursal { get; set; }
        public string SucursalNombre { get; set; } = string.Empty;
        public bool Activo { get; set; }
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUser { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class CreateUsuarioDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Telefono { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public int IdPuesto { get; set; }
        public int IdRol { get; set; }
    }

    public class UpdateUsuarioDto
    {
        [Required(ErrorMessage = "The name is required")]
        [StringLength(50, ErrorMessage = "The name cannot exceed 50 characters")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "The last name is required")]
        [StringLength(50, ErrorMessage = "The last name cannot exceed 50 characters")]
        public string Apellido { get; set; } = string.Empty;

        [Required(ErrorMessage = "The email is required")]
        [StringLength(100, ErrorMessage = "The email cannot exceed 100 characters")]
        public string Email { get; set; } = string.Empty;

        [StringLength(20, ErrorMessage = "The phone number cannot exceed 20 characters")]
        public string? Telefono { get; set; }

        [StringLength(200, ErrorMessage = "The address cannot exceed 200 characters")]
        public string? Direccion { get; set; }

        [Required(ErrorMessage = "The role is required")]
        public int IdRol { get; set; }

        [Required(ErrorMessage = "The department is required")]
        public int IdDepartamento { get; set; }

        [Required(ErrorMessage = "The position is required")]
        public int IdPuesto { get; set; }

        [Required(ErrorMessage = "The branch is required")]
        public int IdSucursal { get; set; }
    }

    public class CambiarContraseñaDto
    {
        [Required(ErrorMessage = "The current password is required")]
        public string ContraseñaActual { get; set; } = string.Empty;

        [Required(ErrorMessage = "The new password is required")]
        [StringLength(100, MinimumLength = 6, ErrorMessage = "The new password must be between 6 and 100 characters")]
        public string NuevaContraseña { get; set; } = string.Empty;

        [Required(ErrorMessage = "The password confirmation is required")]
        [Compare("NuevaContraseña", ErrorMessage = "The password confirmation does not match")]
        public string ConfirmarContraseña { get; set; } = string.Empty;
    }

    public class UsuarioSimpleDto
    {
        public int Id { get; set; }
        public string NombreCompleto { get; set; } = string.Empty;
    }

    public class UsuarioResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string Correo { get; set; } = string.Empty;
        public int? IdPuesto { get; set; }
        public int? IdRol { get; set; }
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }

    public class UsuarioUpdateResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Apellido { get; set; }
        public string? Telefono { get; set; }
        public string Correo { get; set; } = string.Empty;
        public int? IdPuesto { get; set; }
        public int? IdRol { get; set; }
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class UsuarioListDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Correo { get; set; } = string.Empty;
        public string? Puesto { get; set; }
        public string? Rol { get; set; }
        public bool Activo { get; set; }
    }

    public class CreateAdminUsuarioDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 250 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "El apellido no puede exceder los 250 caracteres")]
        public string? Apellido { get; set; }

        [StringLength(13, ErrorMessage = "El teléfono no puede exceder los 13 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [StringLength(250, ErrorMessage = "El correo no puede exceder los 250 caracteres")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(250, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 250 caracteres")]
        public string Contrasena { get; set; } = string.Empty;

        public int IdEmpresa { get; set; }
        public int? IdPuesto { get; set; }
        public int? IdRol { get; set; }
    }

    public class CreateUsuarioSimpleDto
    {
        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(250, MinimumLength = 3, ErrorMessage = "El nombre debe tener entre 3 y 250 caracteres")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(250, ErrorMessage = "El apellido no puede exceder los 250 caracteres")]
        public string? Apellido { get; set; }

        [StringLength(13, ErrorMessage = "El teléfono no puede exceder los 13 caracteres")]
        public string? Telefono { get; set; }

        [Required(ErrorMessage = "El correo electrónico es obligatorio")]
        [StringLength(250, ErrorMessage = "El correo no puede exceder los 250 caracteres")]
        public string Correo { get; set; } = string.Empty;

        [Required(ErrorMessage = "La contraseña es obligatoria")]
        [StringLength(250, MinimumLength = 8, ErrorMessage = "La contraseña debe tener entre 8 y 250 caracteres")]
        public string Contrasena { get; set; } = string.Empty;
    }
} 