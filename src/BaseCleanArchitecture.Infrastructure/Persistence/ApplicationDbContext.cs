using Microsoft.EntityFrameworkCore;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Infrastructure.Persistence.Configurations;

namespace BaseCleanArchitecture.Infrastructure.Persistence
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Departamento> Departamentos { get; set; }
        public DbSet<Sucursal> Sucursales { get; set; }
        public DbSet<Cliente> Clientes { get; set; }
        public DbSet<TipoUnidad> TiposUnidad { get; set; }
        public DbSet<Almacen> Almacenes { get; set; } = null!;
        public DbSet<Marca> Marcas { get; set; } = null!;
        public DbSet<Articulo> Articulos { get; set; } = null!;
        public DbSet<Ubicacion> Ubicaciones { get; set; } = null!;
        public DbSet<Vehiculo> Vehiculos { get; set; } = null!;
        public DbSet<Modelo> Modelos { get; set; } = null!;
        public DbSet<TipoCombustible> TiposCombustible { get; set; } = null!;
        public DbSet<Puesto> Puestos { get; set; } = null!;
        public DbSet<Rol> Roles { get; set; } = null!;
        public DbSet<NotificacionPlantilla> NotificacionPlantillas { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar configuraciones
            modelBuilder.ApplyConfiguration(new DepartamentoConfiguration());

            modelBuilder.Entity<Usuario>()
                .HasIndex(u => u.Correo)
                .IsUnique();

            modelBuilder.Entity<Departamento>()
                .HasIndex(d => d.Nombre)
                .IsUnique();

            modelBuilder.Entity<Sucursal>()
                .HasIndex(s => new { s.Clave, s.IdEmpresa })
                .IsUnique();

            modelBuilder.Entity<Sucursal>()
                .HasIndex(s => new { s.Nombre, s.IdEmpresa })
                .IsUnique();

            modelBuilder.Entity<Cliente>(entity =>
            {
                entity.HasIndex(c => new { c.RazonSocial, c.RFC, c.Direccion }).IsUnique();
            });

            modelBuilder.Entity<TipoUnidad>(entity =>
            {
                entity.HasIndex(t => new { t.Clave, t.IdEmpresa }).IsUnique();
                entity.HasIndex(t => new { t.Nombre, t.IdEmpresa }).IsUnique();
            });

            modelBuilder.Entity<Almacen>(entity =>
            {
                entity.HasIndex(e => new { e.Clave, e.IdEmpresa })
                    .HasDatabaseName("UQ_Almacen_Clave_Empresa")
                    .IsUnique();

                entity.HasIndex(e => new { e.Nombre, e.IdSucursal })
                    .HasDatabaseName("UQ_Almacen_Nombre_Sucursal")
                    .IsUnique();

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Nombre)
                    .HasMaxLength(250);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255);

                entity.HasOne(e => e.Sucursal)
                    .WithMany()
                    .HasForeignKey(e => e.IdSucursal)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.IdUser)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UsuarioUpdate)
                    .WithMany()
                    .HasForeignKey(e => e.IdUserUpdate)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Marca>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.IdUser)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UsuarioUpdate)
                    .WithMany()
                    .HasForeignKey(e => e.IdUserUpdate)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Articulo>(entity =>
            {
                entity.Property(e => e.Nombre)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(255);

                entity.Property(e => e.Material)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Resistencia)
                    .HasMaxLength(255);

                entity.Property(e => e.Duracion)
                    .HasPrecision(4, 2);

                entity.Property(e => e.PrecioUnitario)
                    .HasPrecision(10, 2);

                entity.Property(e => e.PrecioVentanilla)
                    .HasPrecision(10, 2);

                entity.Property(e => e.CodigoBarras)
                    .HasMaxLength(255);

                entity.Property(e => e.Serie)
                    .HasMaxLength(255);

                entity.Property(e => e.PesoPieza)
                    .HasPrecision(10, 2);

                entity.Property(e => e.Lote)
                    .HasMaxLength(255);

                entity.Property(e => e.Factura)
                    .HasMaxLength(255);

                entity.HasIndex(e => new { e.Nombre, e.IdMarca, e.IdModelo })
                    .HasDatabaseName("unq_Nombre_Marca_Modelo")
                    .IsUnique();

                entity.HasOne(e => e.Marca)
                    .WithMany()
                    .HasForeignKey(e => e.IdMarca)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.IdUser)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UsuarioUpdate)
                    .WithMany()
                    .HasForeignKey(e => e.IdUserUpdate)
                    .OnDelete(DeleteBehavior.Restrict);
            });

            modelBuilder.Entity<Vehiculo>(entity =>
            {
                entity.Property(e => e.NumeroEconomico)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Placa)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Serie)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Observaciones)
                    .HasMaxLength(255);

                entity.HasIndex(e => new { e.NumeroEconomico, e.IdEmpresa })
                    .IsUnique();

                entity.HasIndex(e => new { e.Placa, e.IdEmpresa })
                    .IsUnique();

                // NO configurar relaciones para evitar conflictos con la BD
            });

            modelBuilder.Entity<Ubicacion>(entity =>
            {
                entity.Property(e => e.Zona)
                    .HasMaxLength(250);

                entity.Property(e => e.Pasillo)
                    .HasMaxLength(250);

                entity.Property(e => e.Nivel)
                    .HasMaxLength(250);

                entity.Property(e => e.Subnivel)
                    .HasMaxLength(250);

                entity.Property(e => e.Clave)
                    .IsRequired()
                    .HasMaxLength(250);

                entity.Property(e => e.Descripcion)
                    .HasMaxLength(250);

                entity.HasIndex(e => new { e.Clave, e.IdAlmacen })
                    .IsUnique();

                entity.HasOne(e => e.Sucursal)
                    .WithMany()
                    .HasForeignKey(e => e.IdSucursal)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Almacen)
                    .WithMany()
                    .HasForeignKey(e => e.IdAlmacen)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.Usuario)
                    .WithMany()
                    .HasForeignKey(e => e.IdUser)
                    .OnDelete(DeleteBehavior.Restrict);

                entity.HasOne(e => e.UsuarioUpdate)
                    .WithMany()
                    .HasForeignKey(e => e.IdUserUpdate)
                    .OnDelete(DeleteBehavior.Restrict);
            });



            // Apply seed data
            SeedData.Seed(modelBuilder);
        }
    }
} 