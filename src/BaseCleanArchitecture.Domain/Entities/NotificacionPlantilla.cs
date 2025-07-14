using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Domain.Entities
{
    public class NotificacionPlantilla
    {
        public int Id { get; set; }
        
        [Required]
        [MaxLength(100)]
        public string Clave { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(100)]
        public string Modulo { get; set; } = string.Empty;
        
        [Required]
        [MaxLength(200)]
        public string Asunto { get; set; } = string.Empty;
        
        [Required]
        public string ContenidoHTML { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string CorreoEnvio { get; set; } = string.Empty;
        
        [MaxLength(200)]
        public string NombreEnvio { get; set; } = string.Empty;
        
        [MaxLength(500)]
        public string? ConCopia { get; set; }
        
        public bool Activo { get; set; } = true;
        
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }
} 