using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces;

public interface IArticuloRepository
{
    Task<IEnumerable<Articulo>> GetAllAsync();
    Task<Articulo?> GetByIdAsync(int id);
    Task<Articulo?> GetByNombreMarcaModeloAsync(string nombre, string marca, string modelo);
    Task<Articulo> AddAsync(Articulo articulo);
    Task<Articulo> UpdateAsync(Articulo articulo);
    Task<bool> DeleteAsync(int id);
    Task<bool> ExistsAsync(int id);
    Task<bool> ExistsByNombreMarcaModeloAsync(string nombre, string marca, string modelo);
} 