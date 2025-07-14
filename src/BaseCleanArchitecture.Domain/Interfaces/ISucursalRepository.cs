using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces
{
    public interface ISucursalRepository
    {
        Task<IEnumerable<Sucursal>> GetAllAsync();
        Task<IEnumerable<Sucursal>> GetAllActiveAsync();
        Task<Sucursal> GetByIdAsync(int id);
            Task<Sucursal> CreateAsync(Sucursal sucursal);
    Task<Sucursal> UpdateAsync(Sucursal sucursal);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByClaveAndEmpresaAsync(string clave, int idEmpresa);
        Task<bool> ExistsByNombreAndEmpresaAsync(string nombre, int idEmpresa);
        Task<bool> ExistsMatrizInEmpresaAsync(int idEmpresa);
        Task<int> GetIdEmpresaByUserIdAsync(int userId);
        Task<int> GetIdEmpresaByNombreAsync(string nombreEmpresa);
    }
} 