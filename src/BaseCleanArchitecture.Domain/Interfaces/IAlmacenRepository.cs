using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces;

public interface IAlmacenRepository
{
    Task<IEnumerable<Almacen>> GetAllAsync();
    Task<Almacen?> GetByIdAsync(int id);
    Task<Almacen> CreateAsync(Almacen almacen);
    Task<Almacen> UpdateAsync(Almacen almacen);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByClaveAndEmpresaAsync(string clave, int idEmpresa);
    Task<bool> ExistsByNombreAndSucursalAsync(string? nombre, int idSucursal);
    Task<int> GetIdSucursalByNombreAsync(string nombreSucursal);
    Task<int> GetIdEmpresaByUserIdAsync(int userId);
} 