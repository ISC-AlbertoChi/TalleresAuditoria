using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs
{
    public class TipoUnidadDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime FechaCreacion { get; set; }
        public DateTime? FechaModificacion { get; set; }
        public int IdUsuarioCreacion { get; set; }
        public int? IdUsuarioModificacion { get; set; }
        public bool Activo { get; set; }
    }

    public class CreateTipoUnidadDto
    {
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }

    public class UpdateTipoUnidadDto
    {
        [Required(ErrorMessage = "La clave es obligatoria")]
        [StringLength(20, ErrorMessage = "La clave no puede exceder los 20 caracteres")]
        public string Clave { get; set; } = string.Empty;

        [Required(ErrorMessage = "El nombre es obligatorio")]
        [StringLength(100, ErrorMessage = "El nombre no puede exceder los 100 caracteres")]
        public string Nombre { get; set; } = string.Empty;
    }

    public class TipoUnidadResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class CreateTipoUnidadResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }

    public class UpdateTipoUnidadResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string? Nombre { get; set; }
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class TipoUnidadSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
} 