using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Infrastructure.Persistence.Configurations
{
    public class DepartamentoConfiguration : IEntityTypeConfiguration<Departamento>
    {
        public void Configure(EntityTypeBuilder<Departamento> builder)
        {
            builder.ToTable("Departamentos");

            builder.HasKey(d => d.Id);

            builder.Property(d => d.Nombre)
                .IsRequired()
                .HasMaxLength(250);

            builder.Property(d => d.Descripcion)
                .HasMaxLength(250);

            builder.Property(d => d.IdEmpresa)
                .IsRequired(false);

            builder.Property(d => d.Status)
                .IsRequired()
                .HasDefaultValue(true);

            builder.Property(d => d.DateCreate)
                .IsRequired();

            builder.Property(d => d.IdUser)
                .IsRequired(false);

            builder.Property(d => d.IdUserUpdate)
                .IsRequired(false);

            // Índices - solo en Nombre, sin IdEmpresa para permitir departamentos sin empresa
            builder.HasIndex(d => d.Nombre)
                .IsUnique();
        }
    }
} 