using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class MarcaRepository : IMarcaRepository
{
    private readonly ApplicationDbContext _context;

    public MarcaRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Marca>> GetAllAsync()
    {
        return await _context.Marcas
            .ToListAsync();
    }

    public async Task<IEnumerable<Marca>> GetAllActiveAsync()
    {
        return await _context.Marcas
            .Where(m => m.Status)
            .ToListAsync();
    }

    public async Task<Marca?> GetByIdAsync(int id)
    {
        return await _context.Marcas
            .FirstOrDefaultAsync(m => m.Id == id && m.Status);
    }

    public async Task<Marca> CreateAsync(Marca marca)
    {
        _context.Marcas.Add(marca);
        await _context.SaveChangesAsync();
        return marca;
    }

    public async Task<Marca> UpdateAsync(Marca marca)
    {
        _context.Entry(marca).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return marca;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var marca = await _context.Marcas.FindAsync(id);
        if (marca == null) return false;

        marca.Status = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByNombreAndEmpresaAsync(string nombre, int idEmpresa)
    {
        return await _context.Marcas
            .AnyAsync(m => m.Nombre == nombre && m.IdEmpresa == idEmpresa && m.Status);
    }

    public async Task<bool> ExistsByNombreAsync(string nombre, int idEmpresa)
    {
        return await _context.Marcas
            .AnyAsync(m => m.Nombre == nombre && m.IdEmpresa == idEmpresa && m.Status);
    }

    public async Task<int> GetIdEmpresaByUserIdAsync(int userId)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == userId);

        return usuario?.IdEmpresa ?? 0;
    }

    public async Task<Marca?> GetByNombreAsync(string nombre)
    {
        return await _context.Marcas
            .FirstOrDefaultAsync(m => m.Nombre == nombre && m.Status);
    }
} 