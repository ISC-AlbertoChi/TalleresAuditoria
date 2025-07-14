namespace BaseCleanArchitecture.Application.DTOs
{
    public class MarcaDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public DateTime? DateCreate { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUser { get; set; }
        public int? IdUserUpdate { get; set; }
        public bool Activo { get; set; }
    }

    public class CreateMarcaDto
    {
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }

    public class UpdateMarcaDto
    {
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
    }

    public class MarcaResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class CreateMarcaResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
    }

    public class UpdateMarcaResponseDto
    {
        public int Id { get; set; }
        public string Clave { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public int? IdEmpresa { get; set; }
        public bool Status { get; set; }
        public DateTime? DateCreate { get; set; }
        public int? IdUser { get; set; }
        public DateTime? DateUpdate { get; set; }
        public int? IdUserUpdate { get; set; }
    }

    public class MarcaSimpleDto
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
    }
} 