using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Application.DTOs
{
    public class DepartamentoDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public DateTime DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int IdUser { get; set; }
        public int? IdUserUpdate { get; set; }
        public bool Activo { get; set; }
    }

    public class CreateDepartamentoDto
    {
        public string Nombre { get; set; } = string.Empty;
        public string Descripcion { get; set; } = string.Empty;
    }

    public class UpdateDepartamentoDto
    {
        [Required(ErrorMessage = "The name is required")]
        [StringLength(100, ErrorMessage = "The name cannot exceed 100 characters")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(255, ErrorMessage = "The description cannot exceed 255 characters")]
        public string? Descripcion { get; set; }
    }

    public class DepartamentoResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }

    public class DepartamentoUpdateResponseDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class DepartamentoSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
} 