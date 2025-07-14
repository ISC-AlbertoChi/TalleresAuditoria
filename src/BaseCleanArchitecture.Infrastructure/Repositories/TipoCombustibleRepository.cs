using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class TipoCombustibleRepository : ITipoCombustibleRepository
{
    private readonly ApplicationDbContext _context;

    public TipoCombustibleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TipoCombustible>> GetAllAsync()
    {
        return await _context.TiposCombustible
            .Where(t => t.Status)
            .OrderBy(t => t.Nombre)
            .ToListAsync();
    }

    public async Task<TipoCombustible?> GetByIdAsync(int id)
    {
        return await _context.TiposCombustible
            .FirstOrDefaultAsync(t => t.Id == id && t.Status);
    }
} 