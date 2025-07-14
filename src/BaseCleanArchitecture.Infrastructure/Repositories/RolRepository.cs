using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class RolRepository : IRolRepository
{
    private readonly ApplicationDbContext _context;

    public RolRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Rol>> GetAllAsync()
    {
        return await _context.Roles
            .Where(r => r.Status)
            .OrderBy(r => r.Nombre)
            .ToListAsync();
    }

    public async Task<Rol?> GetByIdAsync(int id)
    {
        return await _context.Roles
            .FirstOrDefaultAsync(r => r.Id == id && r.Status);
    }
} 