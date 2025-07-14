using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories
{
    public class TipoUnidadRepository : ITipoUnidadRepository
    {
        private readonly ApplicationDbContext _context;

        public TipoUnidadRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<TipoUnidad>> GetAllAsync()
        {
            return await _context.TiposUnidad
                .Where(t => t.Status)
                .ToListAsync();
        }

        public async Task<TipoUnidad?> GetByIdAsync(int id)
        {
            return await _context.TiposUnidad
                .FirstOrDefaultAsync(t => t.Id == id && t.Status);
        }

        public async Task<TipoUnidad> CreateAsync(TipoUnidad tipoUnidad)
        {
            _context.TiposUnidad.Add(tipoUnidad);
            await _context.SaveChangesAsync();
            return tipoUnidad;
        }

        public async Task<TipoUnidad> UpdateAsync(TipoUnidad tipoUnidad)
        {
            _context.Entry(tipoUnidad).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return tipoUnidad;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var tipoUnidad = await _context.TiposUnidad.FindAsync(id);
            if (tipoUnidad == null)
                return false;

            tipoUnidad.Status = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByClaveAsync(string clave, int idEmpresa)
        {
            return await _context.TiposUnidad
                .AnyAsync(t => t.Clave == clave && t.IdEmpresa == idEmpresa && t.Status);
        }

        public async Task<bool> ExistsByNombreAsync(string nombre, int idEmpresa)
        {
            return await _context.TiposUnidad
                .AnyAsync(t => t.Nombre == nombre && t.IdEmpresa == idEmpresa && t.Status);
        }

        public async Task<int> GetIdEmpresaByUserIdAsync(int userId)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == userId);
            return usuario?.IdEmpresa ?? 0;
        }

        public async Task<int> GetIdEmpresaByNombreAsync(string nombreEmpresa)
        {
            return 0;
        }
    }
} 