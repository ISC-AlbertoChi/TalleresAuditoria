using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces;

public interface IPuestoRepository
{
    Task<IEnumerable<Puesto>> GetAllAsync();
    Task<Puesto?> GetByIdAsync(int id);
} 