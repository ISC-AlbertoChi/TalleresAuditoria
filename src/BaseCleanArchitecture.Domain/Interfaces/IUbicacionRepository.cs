using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces;

public interface IUbicacionRepository
{
    Task<IEnumerable<Ubicacion>> GetAllAsync();
    Task<Ubicacion?> GetByIdAsync(int id);
    Task<Ubicacion> AddAsync(Ubicacion ubicacion);
    Task<Ubicacion> UpdateAsync(Ubicacion ubicacion);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByClaveAndAlmacenAsync(string clave, int idAlmacen);
    Task<int> GetIdSucursalByNombreAsync(string nombreSucursal);
    Task<int> GetIdAlmacenByNombreAsync(string nombreAlmacen);
    Task<int> GetIdEmpresaByNombreAsync(string nombreEmpresa);
    Task<bool> HasArticulosActivosAsync(int idUbicacion);
    Task<int> GetIdEmpresaByUserIdAsync(int userId);
} 