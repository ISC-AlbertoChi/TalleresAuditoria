using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class AlmacenRepository : IAlmacenRepository
{
    private readonly ApplicationDbContext _context;

    public AlmacenRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Almacen>> GetAllAsync()
    {
        return await _context.Almacenes
            .Include(a => a.Sucursal)
            .Where(a => a.Status)
            .ToListAsync();
    }

    public async Task<Almacen?> GetByIdAsync(int id)
    {
        return await _context.Almacenes
            .Include(a => a.Sucursal)
            .FirstOrDefaultAsync(a => a.Id == id && a.Status);
    }

    public async Task<Almacen> CreateAsync(Almacen almacen)
    {
        _context.Almacenes.Add(almacen);
        await _context.SaveChangesAsync();
        return almacen;
    }



    public async Task<Almacen> UpdateAsync(Almacen almacen)
    {
        _context.Entry(almacen).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return almacen;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var almacen = await _context.Almacenes.FindAsync(id);
        if (almacen == null) return false;

        almacen.Status = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByClaveAndEmpresaAsync(string clave, int idEmpresa)
    {
        return await _context.Almacenes
            .AnyAsync(a => a.Clave == clave && a.IdEmpresa == idEmpresa && a.Status);
    }

    public async Task<bool> ExistsByNombreAndSucursalAsync(string? nombre, int idSucursal)
    {
        if (string.IsNullOrEmpty(nombre)) return false;
        return await _context.Almacenes
            .AnyAsync(a => a.Nombre == nombre && a.IdSucursal == idSucursal && a.Status);
    }

    public async Task<int> GetIdSucursalByNombreAsync(string nombreSucursal)
    {
        var sucursal = await _context.Sucursales
            .FirstOrDefaultAsync(s => s.Nombre == nombreSucursal && s.Status);
        
        if (sucursal == null)
            throw new Exception($"No se encontró la sucursal con nombre: {nombreSucursal}");
        
        return sucursal.Id;
    }

    public async Task<int> GetIdEmpresaByUserIdAsync(int userId)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == userId && u.Status);
        
        if (usuario == null)
            throw new Exception($"No se encontró el usuario con ID: {userId}");
        
        return usuario.IdEmpresa ?? 0;
    }
} 