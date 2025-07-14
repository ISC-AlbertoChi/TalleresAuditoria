using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;

namespace BaseCleanArchitecture.Infrastructure.Repositories
{
    public class UsuarioRepository : GenericRepository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<Usuario>> GetAllActiveAsync()
        {
            return await _context.Usuarios
                .Where(u => u.Status)
                .ToListAsync();
        }

        public new async Task<Usuario> AddAsync(Usuario usuario)
        {
            await _context.Usuarios.AddAsync(usuario);
            await _context.SaveChangesAsync();
            
            // Recargar el usuario con sus relaciones usando el ID generado
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == usuario.Id);
        }

        public async Task UpdateAsync(Usuario usuario)
        {
            _context.Entry(usuario).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }

        public async Task DeleteAsync(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario != null)
            {
                usuario.Status = false;
                await _context.SaveChangesAsync();
            }
        }

        public async Task<bool> ExistsAsync(int id)
        {
            return await _context.Usuarios.AnyAsync(u => u.Id == id);
        }

        public async Task<bool> HasActiveRelationsAsync(int id)
        {
            return await _context.Usuarios
                .AnyAsync(u => u.IdUser == id && u.Status);
        }

        public async Task<int> GetEmpresaIdByUserIdAsync(int userId)
        {
            var usuario = await _context.Usuarios
                .Where(u => u.Id == userId)
                .Select(u => u.IdEmpresa)
                .FirstOrDefaultAsync();
            return usuario ?? 0;
        }

        public async Task<Usuario> GetByEmailAsync(string email)
        {
            return await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Correo == email && u.Status);
        }
    }
} 