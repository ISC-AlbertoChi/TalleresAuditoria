using BaseCleanArchitecture.Domain.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BaseCleanArchitecture.Domain.Interfaces;

public interface IVehiculoRepository
{
    Task<List<Vehiculo>> GetAllAsync();
    Task<Vehiculo?> GetByIdAsync(int id);
    Task<Vehiculo> CreateAsync(Vehiculo vehiculo);
    Task UpdateAsync(Vehiculo vehiculo);
    Task DeleteAsync(int id);
    Task<bool> ExistsByNumeroEconomicoAsync(string numeroEconomico, int idEmpresa);
    Task<bool> ExistsByPlacaAsync(string placa, int idEmpresa);
    Task<int> GetIdEmpresaByUserIdAsync(int userId);
    Task<IEnumerable<Vehiculo>> GetByEmpresaAsync(int idEmpresa);

    // Métodos para obtener con relaciones
    Task<List<Vehiculo>> GetAllWithRelationsAsync();
    Task<Vehiculo?> GetByIdWithRelationsAsync(int id);
} 