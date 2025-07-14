using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces
{
    public interface IDepartamentoRepository
    {
        Task<IEnumerable<Departamento>> GetAllAsync();
        Task<Departamento> GetByIdAsync(int id);
            Task<Departamento> CreateAsync(Departamento departamento);
    Task<Departamento> UpdateAsync(Departamento departamento);
        Task<bool> DeleteAsync(int id);
        Task<bool> ExistsByNombreAndEmpresaAsync(string nombre, int? idEmpresa);
        Task<int> GetIdEmpresaByUserIdAsync(int userId);
    }
} 