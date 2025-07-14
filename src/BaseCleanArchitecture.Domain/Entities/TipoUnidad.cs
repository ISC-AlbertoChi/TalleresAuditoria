using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCleanArchitecture.Domain.Entities
{
    public class TipoUnidad
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(250, MinimumLength = 3)]
        public string Clave { get; set; }

        [StringLength(250, MinimumLength = 3)]
        public string? Nombre { get; set; }

        public int? IdEmpresa { get; set; }

        public bool Status { get; set; } = true;

        public DateTime? DateCreate { get; set; }

        public int? IdUser { get; set; }

        public DateTime? DateUpdate { get; set; }

        public int? IdUserUpdate { get; set; }

        // Empresa se maneja como entidad de apoyo

        [ForeignKey("IdUser")]
        public virtual Usuario? UsuarioCreacion { get; set; }

        [ForeignKey("IdUserUpdate")]
        public virtual Usuario? UsuarioActualizacion { get; set; }
    }
} 