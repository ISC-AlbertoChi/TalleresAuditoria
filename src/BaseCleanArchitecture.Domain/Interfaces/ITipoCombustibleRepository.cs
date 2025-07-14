using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces;

public interface ITipoCombustibleRepository
{
    Task<IEnumerable<TipoCombustible>> GetAllAsync();
    Task<TipoCombustible?> GetByIdAsync(int id);
} 