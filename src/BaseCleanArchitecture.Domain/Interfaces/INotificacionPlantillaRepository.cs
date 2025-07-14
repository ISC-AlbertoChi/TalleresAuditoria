using BaseCleanArchitecture.Domain.Entities;

namespace BaseCleanArchitecture.Domain.Interfaces
{
    public interface INotificacionPlantillaRepository : IGenericRepository<NotificacionPlantilla>
    {
        Task<NotificacionPlantilla?> GetByKeyAndModuleAsync(string clave, string modulo);
    }
} 