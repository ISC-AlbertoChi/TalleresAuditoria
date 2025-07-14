using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class VehiculoRepository : IVehiculoRepository
{
    private readonly ApplicationDbContext _context;

    public VehiculoRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<List<Vehiculo>> GetAllAsync()
    {
        return await _context.Vehiculos
            .Where(v => v.Status)
            .ToListAsync();
    }

    public async Task<Vehiculo?> GetByIdAsync(int id)
    {
        return await _context.Vehiculos
            .FirstOrDefaultAsync(v => v.Id == id && v.Status);
    }

    public async Task<Vehiculo> CreateAsync(Vehiculo vehiculo)
    {
        _context.Vehiculos.Add(vehiculo);
        await _context.SaveChangesAsync();
        return vehiculo;
    }

    public async Task UpdateAsync(Vehiculo vehiculo)
    {
        _context.Entry(vehiculo).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        var vehiculo = await _context.Vehiculos.FindAsync(id);
        if (vehiculo != null)
        {
            vehiculo.Status = false;
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> ExistsByNumeroEconomicoAsync(string numeroEconomico, int idEmpresa)
    {
        return await _context.Vehiculos
            .AnyAsync(v => v.NumeroEconomico == numeroEconomico && v.IdEmpresa == idEmpresa && v.Status);
    }

    public async Task<bool> ExistsByPlacaAsync(string placa, int idEmpresa)
    {
        return await _context.Vehiculos
            .AnyAsync(v => v.Placa == placa && v.IdEmpresa == idEmpresa && v.Status);
    }

    public async Task<int> GetIdEmpresaByUserIdAsync(int userId)
    {
        var usuario = await _context.Usuarios.FirstOrDefaultAsync(u => u.Id == userId);
        return usuario?.IdEmpresa ?? 0;
    }

    public async Task<IEnumerable<Vehiculo>> GetByEmpresaAsync(int idEmpresa)
    {
        return await _context.Vehiculos
            .Where(v => v.IdEmpresa == idEmpresa && v.Status)
            .ToListAsync();
    }

    public async Task<List<Vehiculo>> GetAllWithRelationsAsync()
    {
        return await _context.Vehiculos
            .ToListAsync();
    }

    public async Task<Vehiculo?> GetByIdWithRelationsAsync(int id)
    {
        return await _context.Vehiculos
            .Include(v => v.Marca)
            .Include(v => v.Propietario)
            .Include(v => v.TipoUnidad)
            .FirstOrDefaultAsync(v => v.Id == id);
    }
} 