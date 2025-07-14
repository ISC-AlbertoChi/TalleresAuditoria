using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BaseCleanArchitecture.Infrastructure.Persistence;

namespace BaseCleanArchitecture.Infrastructure.Repositories
{
    public class DepartamentoRepository : IDepartamentoRepository
    {
        private readonly ApplicationDbContext _context;

        public DepartamentoRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Departamento>> GetAllAsync()
        {
            return await _context.Departamentos
                .Where(d => d.Status)
                .ToListAsync();
        }

        public async Task<Departamento> GetByIdAsync(int id)
        {
            return await _context.Departamentos
                .FirstOrDefaultAsync(d => d.Id == id && d.Status);
        }

        public async Task<Departamento> CreateAsync(Departamento departamento)
        {
            _context.Departamentos.Add(departamento);
            await _context.SaveChangesAsync();
            return departamento;
        }

        public async Task<Departamento> UpdateAsync(Departamento departamento)
        {
            _context.Entry(departamento).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return departamento;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var departamento = await _context.Departamentos.FindAsync(id);
            if (departamento == null) return false;

            departamento.Status = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByNombreAndEmpresaAsync(string nombre, int? idEmpresa)
        {
            return await _context.Departamentos
                .AnyAsync(d => d.Nombre == nombre && d.IdEmpresa == idEmpresa && d.Status);
        }

        public async Task<int> GetIdEmpresaByUserIdAsync(int userId)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == userId && u.Status);

            if (usuario?.IdEmpresa == null)
                throw new Exception($"No se encontró la empresa para el usuario con ID: {userId}");

            return usuario.IdEmpresa.Value;
        }
    }
} 