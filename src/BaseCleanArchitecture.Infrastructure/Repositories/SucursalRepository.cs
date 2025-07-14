using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BaseCleanArchitecture.Infrastructure.Persistence;

namespace BaseCleanArchitecture.Infrastructure.Repositories
{
    public class SucursalRepository : ISucursalRepository
    {
        private readonly ApplicationDbContext _context;

        public SucursalRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Sucursal>> GetAllAsync()
        {
            return await _context.Sucursales
                .Where(s => s.Status)
                .ToListAsync();
        }

        public async Task<IEnumerable<Sucursal>> GetAllActiveAsync()
        {
            return await _context.Sucursales
                .Where(s => s.Status)
                .ToListAsync();
        }

        public async Task<Sucursal> GetByIdAsync(int id)
        {
            return await _context.Sucursales
                .FirstOrDefaultAsync(s => s.Id == id && s.Status);
        }

        public async Task<Sucursal> CreateAsync(Sucursal sucursal)
        {
            _context.Sucursales.Add(sucursal);
            await _context.SaveChangesAsync();
            return sucursal;
        }

        public async Task<Sucursal> UpdateAsync(Sucursal sucursal)
        {
            _context.Entry(sucursal).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return sucursal;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var sucursal = await _context.Sucursales.FindAsync(id);
            if (sucursal == null) return false;

            sucursal.Status = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByClaveAndEmpresaAsync(string clave, int idEmpresa)
        {
            return await _context.Sucursales
                .AnyAsync(s => s.Clave == clave && s.IdEmpresa == idEmpresa && s.Status);
        }

        public async Task<bool> ExistsByNombreAndEmpresaAsync(string nombre, int idEmpresa)
        {
            return await _context.Sucursales
                .AnyAsync(s => s.Nombre == nombre && s.IdEmpresa == idEmpresa && s.Status);
        }

        public async Task<bool> ExistsMatrizInEmpresaAsync(int idEmpresa)
        {
            return await _context.Sucursales
                .AnyAsync(s => s.IdEmpresa == idEmpresa && s.EsMatriz && s.Status);
        }

        public async Task<int> GetIdEmpresaByUserIdAsync(int userId)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == userId && u.Status);
            return usuario?.IdEmpresa ?? 0;
        }

        public async Task<int> GetIdEmpresaByNombreAsync(string nombreEmpresa)
        {
            return 0;
        }
    }
} 