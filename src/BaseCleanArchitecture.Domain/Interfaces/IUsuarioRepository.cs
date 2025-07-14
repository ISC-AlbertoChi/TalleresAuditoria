using System.Collections.Generic;
using System.Threading.Tasks;
using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces
{
    public interface IUsuarioRepository : IGenericRepository<Usuario>
    {
        Task<IEnumerable<Usuario>> GetAllActiveAsync();
        Task<Usuario> AddAsync(Usuario usuario);
        Task UpdateAsync(Usuario usuario);
        Task DeleteAsync(int id);
        Task<bool> ExistsAsync(int id);
        Task<bool> HasActiveRelationsAsync(int id);
        Task<int> GetEmpresaIdByUserIdAsync(int userId);
        Task<Usuario> GetByEmailAsync(string email);
    }
} 