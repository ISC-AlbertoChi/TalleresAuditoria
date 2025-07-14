using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class PuestoRepository : IPuestoRepository
{
    private readonly ApplicationDbContext _context;

    public PuestoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Puesto>> GetAllAsync()
    {
        return await _context.Puestos
            .Where(p => p.Status)
            .OrderBy(p => p.Nombre)
            .ToListAsync();
    }

    public async Task<Puesto?> GetByIdAsync(int id)
    {
        return await _context.Puestos
            .FirstOrDefaultAsync(p => p.Id == id && p.Status);
    }
} 