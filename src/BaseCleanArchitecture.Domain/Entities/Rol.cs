using System.ComponentModel.DataAnnotations;

namespace BaseCleanArchitecture.Domain.Entities;

public class Rol
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = string.Empty;
    
    [StringLength(255)]
    public string? Descripcion { get; set; }
    
    public int? IdEmpresa { get; set; }
    
    public bool Status { get; set; } = true;
    
    public DateTime? DateCreate { get; set; }
    
    public int? IdUser { get; set; }
    
    public DateTime? DateUpdate { get; set; }
    
    public int? IdUserUpdate { get; set; }
} 