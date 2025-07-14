using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class ArticuloRepository : IArticuloRepository
{
    private readonly ApplicationDbContext _context;

    public ArticuloRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Articulo>> GetAllAsync()
    {
        return await _context.Articulos
            .Include(a => a.Marca)
            .ToListAsync();
    }

    public async Task<Articulo?> GetByIdAsync(int id)
    {
        return await _context.Articulos
            .Include(a => a.Marca)
            .FirstOrDefaultAsync(a => a.Id == id);
    }

    public async Task<Articulo?> GetByNombreMarcaModeloAsync(string nombre, string marca, string modelo)
    {
        return await _context.Articulos
            .Include(a => a.Marca)
            .FirstOrDefaultAsync(a => 
                a.Nombre == nombre && 
                a.Marca.Nombre == marca && 
                a.IdModelo.ToString() == modelo);
    }

    public async Task<Articulo> AddAsync(Articulo articulo)
    {
        await _context.Articulos.AddAsync(articulo);
        await _context.SaveChangesAsync();
        return articulo;
    }

    public async Task<Articulo> UpdateAsync(Articulo articulo)
    {
        _context.Articulos.Update(articulo);
        await _context.SaveChangesAsync();
        return articulo;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var articulo = await _context.Articulos.FindAsync(id);
        if (articulo == null)
            return false;

        articulo.Status = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Articulos.AnyAsync(a => a.Id == id);
    }

    public async Task<bool> ExistsByNombreMarcaModeloAsync(string nombre, string marca, string modelo)
    {
        return await _context.Articulos
            .Include(a => a.Marca)
            .AnyAsync(a => 
                a.Nombre == nombre && 
                a.Marca.Nombre == marca && 
                a.IdModelo.ToString() == modelo);
    }
} 