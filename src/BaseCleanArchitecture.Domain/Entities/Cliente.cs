using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCleanArchitecture.Domain.Entities
{
    public class Cliente
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(255)]
        public string NombreComercial { get; set; } = null!;

        [Required]
        [StringLength(255)]
        public string RazonSocial { get; set; } = null!;

        [Required]
        [StringLength(13)]
        [RegularExpression(@"^[A-Z]{3,4}[0-9]{6}[A-Z0-9]{3}$", ErrorMessage = "El RFC no tiene el formato correcto")]
        public string RFC { get; set; } = null!;

        [StringLength(255)]
        public string? Direccion { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? Correo { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[0-9]{10,20}$", ErrorMessage = "El teléfono debe tener entre 10 y 20 dígitos")]
        public string? Telefono { get; set; }

        [StringLength(255)]
        public string? NombreContacto { get; set; }

        [StringLength(20)]
        [RegularExpression(@"^[0-9]{10,20}$", ErrorMessage = "El teléfono de contacto debe tener entre 10 y 20 dígitos")]
        public string? TelefonoContacto { get; set; }

        [StringLength(255)]
        [EmailAddress]
        public string? CorreoContacto { get; set; }

        public bool Status { get; set; } = true;

        public DateTime? DateCreate { get; set; }

        public int? IdUser { get; set; }

        public DateTime? DateUpdate { get; set; }

        public int? IdUserUpdate { get; set; }

        // Empresa se maneja como entidad de apoyo

        [ForeignKey("IdUser")]
        public virtual Usuario? Usuario { get; set; }

        [ForeignKey("IdUserUpdate")]
        public virtual Usuario? UsuarioUpdate { get; set; }
    }
} 