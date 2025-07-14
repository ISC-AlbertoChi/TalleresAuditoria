using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces;

public interface IModeloRepository
{
    Task<IEnumerable<Modelo>> GetAllAsync();
    Task<Modelo?> GetByNombreAsync(string nombre);
    Task<Modelo> AddAsync(Modelo modelo);
} 