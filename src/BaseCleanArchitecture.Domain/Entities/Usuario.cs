using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCleanArchitecture.Domain.Entities
{
    public class Usuario
    {
        public int Id { get; set; }

        [Required]
        [MinLength(3)]
        [MaxLength(250)]
        public string Nombre { get; set; } = null!;

        [MaxLength(250)]
        public string? Apellido { get; set; }

        [MaxLength(13)]
        [RegularExpression(@"^[0-9]{10,13}$", ErrorMessage = "El teléfono debe tener entre 10 y 13 dígitos")]
        public string? Telefono { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(250)]
        public string Correo { get; set; } = null!;

        [Required]
        [MinLength(8)]
        [MaxLength(250)]
        public string Contrasena { get; set; } = null!;

        public int? IdPuesto { get; set; }
        public int? IdRol { get; set; }
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; } = true;
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }

        // Navigation properties - Empresa se maneja como entidad de apoyo
    }
} 