using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces
{
    public interface ITipoUnidadRepository
    {
        Task<IEnumerable<TipoUnidad>> GetAllAsync();
        Task<TipoUnidad?> GetByIdAsync(int id);
        Task<TipoUnidad> CreateAsync(TipoUnidad tipoUnidad);
        Task<TipoUnidad> UpdateAsync(TipoUnidad tipoUnidad);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByClaveAsync(string clave, int idEmpresa);
        Task<bool> ExistsByNombreAsync(string nombre, int idEmpresa);
        Task<int> GetIdEmpresaByUserIdAsync(int userId);
        Task<int> GetIdEmpresaByNombreAsync(string nombreEmpresa);
    }
} 