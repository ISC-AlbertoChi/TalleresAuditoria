using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces;

public interface IMarcaRepository
{
    Task<IEnumerable<Marca>> GetAllAsync();
    Task<Marca?> GetByIdAsync(int id);
    Task<Marca> CreateAsync(Marca marca);
    Task<Marca> UpdateAsync(Marca marca);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsByNombreAndEmpresaAsync(string nombre, int idEmpresa);
    Task<int> GetIdEmpresaByUserIdAsync(int userId);
    Task<Marca?> GetByNombreAsync(string nombre);
} 