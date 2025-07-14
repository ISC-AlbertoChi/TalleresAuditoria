using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BaseCleanArchitecture.Domain.Entities;

public class Modelo
{
    [Key]
    public int Id { get; set; }

    [Required]
    [StringLength(100)]
    public string Nombre { get; set; } = null!;

    [StringLength(255)]
    public string? Descripcion { get; set; }

    public bool Status { get; set; } = true;

    public DateTime? DateCreate { get; set; }

    public int? IdUser { get; set; }

    public DateTime? DateUpdate { get; set; }

    public int? IdUserUpdate { get; set; }

    // Relaciones
    [ForeignKey("IdUser")]
    public virtual Usuario? Usuario { get; set; }

    [ForeignKey("IdUserUpdate")]
    public virtual Usuario? UsuarioUpdate { get; set; }
} 