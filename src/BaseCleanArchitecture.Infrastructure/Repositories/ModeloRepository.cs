using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class ModeloRepository : IModeloRepository
{
    private readonly ApplicationDbContext _context;

    public ModeloRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Modelo>> GetAllAsync()
    {
        return await _context.Modelos
            .Where(m => m.Status)
            .Select(m => new Modelo { Id = m.Id, Nombre = m.Nombre })
            .ToListAsync();
    }

    public async Task<Modelo?> GetByIdAsync(int id)
    {
        return await _context.Modelos
            .FirstOrDefaultAsync(m => m.Id == id && m.Status);
    }

    public async Task<Modelo> AddAsync(Modelo modelo)
    {
        await _context.Modelos.AddAsync(modelo);
        await _context.SaveChangesAsync();
        return modelo;
    }

    public async Task AddRangeAsync(IEnumerable<Modelo> entities)
    {
        await _context.Modelos.AddRangeAsync(entities);
        await _context.SaveChangesAsync();
    }

    public async Task<Modelo> UpdateAsync(Modelo modelo)
    {
        _context.Modelos.Update(modelo);
        await _context.SaveChangesAsync();
        return modelo;
    }

    public void Update(Modelo entity)
    {
        _context.Modelos.Update(entity);
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var modelo = await _context.Modelos.FindAsync(id);
        if (modelo == null)
            return false;

        modelo.Status = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public void Remove(Modelo entity)
    {
        _context.Modelos.Remove(entity);
    }

    public void RemoveRange(IEnumerable<Modelo> entities)
    {
        _context.Modelos.RemoveRange(entities);
    }

    public async Task<bool> ExistsAsync(int id)
    {
        return await _context.Modelos.AnyAsync(m => m.Id == id && m.Status);
    }

    public async Task<Modelo?> FindAsync(Expression<Func<Modelo, bool>> predicate)
    {
        return await _context.Modelos.FirstOrDefaultAsync(predicate);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<Modelo?> GetByNombreAsync(string nombre)
    {
        return await _context.Modelos
            .FirstOrDefaultAsync(m => m.Nombre == nombre && m.Status);
    }

    public async Task<bool> ExistsByNombreAsync(string nombre)
    {
        return await _context.Modelos
            .AnyAsync(m => m.Nombre == nombre && m.Status);
    }
} 