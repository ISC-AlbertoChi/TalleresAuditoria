using Microsoft.EntityFrameworkCore;
using BaseCleanArchitecture.Domain.Entities;
using System.Security.Cryptography;
using System.Text;
using BCrypt.Net;

namespace BaseCleanArchitecture.Infrastructure.Persistence.Configurations
{
    public static class SeedData
    {
        public static void Seed(ModelBuilder modelBuilder)
        {
            // Ejemplo: SÓLO entidades reales
            // 1. Departamento
            modelBuilder.Entity<Departamento>().HasData(
                new Departamento
                {
                    Id = 1,
                    Nombre = "General",
                    IdEmpresa = 1,
                    Status = true,
                    DateCreate = DateTime.UtcNow,
                    IdUser = 1,
                    IdUserUpdate = 1
                }
            );

            // 2. Usuario Admin
            modelBuilder.Entity<Usuario>().HasData(
                new Usuario
                {
                    Id = 1,
                    Nombre = "Admin",
                    Apellido = "Sistema",
                    Correo = "admin@iespro.com",
                    Contrasena = BCrypt.Net.BCrypt.HashPassword("Admin123!"),
                    Telefono = "1234567890",
                    IdEmpresa = 1,
                    IdPuesto = 1,
                    Status = true,
                    IdUser = 1,
                    IdUserUpdate = 1
                }
            );
        }
    }
} 