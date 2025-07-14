using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories;

public class UbicacionRepository : IUbicacionRepository
{
    private readonly ApplicationDbContext _context;

    public UbicacionRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Ubicacion>> GetAllAsync()
    {
        return await _context.Ubicaciones
            .Include(u => u.Sucursal)
            .Include(u => u.Almacen)
            .Where(u => u.Status)
            .ToListAsync();
    }

    public async Task<Ubicacion?> GetByIdAsync(int id)
    {
        return await _context.Ubicaciones
            .Include(u => u.Sucursal)
            .Include(u => u.Almacen)
            .FirstOrDefaultAsync(u => u.Id == id && u.Status);
    }

    public async Task<Ubicacion> AddAsync(Ubicacion ubicacion)
    {
        await _context.Ubicaciones.AddAsync(ubicacion);
        await _context.SaveChangesAsync();
        return ubicacion;
    }

    public async Task<Ubicacion> UpdateAsync(Ubicacion ubicacion)
    {
        _context.Entry(ubicacion).State = EntityState.Modified;
        await _context.SaveChangesAsync();
        return ubicacion;
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var ubicacion = await _context.Ubicaciones.FindAsync(id);
        if (ubicacion == null) return false;

        ubicacion.Status = false;
        await _context.SaveChangesAsync();
        return true;
    }

    public async Task<bool> ExistsByClaveAndAlmacenAsync(string clave, int idAlmacen)
    {
        return await _context.Ubicaciones
            .AnyAsync(u => u.Clave == clave && u.IdAlmacen == idAlmacen && u.Status);
    }

    public async Task<int> GetIdSucursalByNombreAsync(string nombreSucursal)
    {
        var sucursal = await _context.Sucursales
            .FirstOrDefaultAsync(s => s.Nombre == nombreSucursal && s.Status);
        return sucursal?.Id ?? 0;
    }

    public async Task<int> GetIdAlmacenByNombreAsync(string nombreAlmacen)
    {
        var almacen = await _context.Almacenes
            .FirstOrDefaultAsync(a => a.Nombre == nombreAlmacen && a.Status);
        return almacen?.Id ?? 0;
    }

    public async Task<int> GetIdEmpresaByNombreAsync(string nombreEmpresa)
    {
        // Eliminado: no existe _context.Empresas ni entidad Empresa
        return 0;
    }

    public Task<bool> HasArticulosActivosAsync(int idUbicacion)
    {
        // Aquí deberías implementar la lógica para verificar si hay artículos activos
        // asociados a esta ubicación. Por ahora retornamos false.
        return Task.FromResult(false);
    }

    public async Task<int> GetIdEmpresaByUserIdAsync(int userId)
    {
        var usuario = await _context.Usuarios
            .FirstOrDefaultAsync(u => u.Id == userId && u.Status);
        return usuario?.IdEmpresa ?? 0;
    }
} 