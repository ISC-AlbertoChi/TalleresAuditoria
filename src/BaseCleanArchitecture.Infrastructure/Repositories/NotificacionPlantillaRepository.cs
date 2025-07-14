using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using BaseCleanArchitecture.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace BaseCleanArchitecture.Infrastructure.Repositories
{
    public class NotificacionPlantillaRepository : GenericRepository<NotificacionPlantilla>, INotificacionPlantillaRepository
    {
        public NotificacionPlantillaRepository(ApplicationDbContext context) : base(context)
        {
        }

        public async Task<NotificacionPlantilla?> GetByKeyAndModuleAsync(string clave, string modulo)
        {
            return await _context.Set<NotificacionPlantilla>()
                .FirstOrDefaultAsync(t => t.Clave == clave && t.Modulo == modulo && t.Activo);
        }
    }
} 