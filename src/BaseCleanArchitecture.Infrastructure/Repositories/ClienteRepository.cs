using BaseCleanArchitecture.Domain.Entities;
using BaseCleanArchitecture.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using BaseCleanArchitecture.Infrastructure.Persistence;

namespace BaseCleanArchitecture.Infrastructure.Repositories
{
    public class ClienteRepository : IClienteRepository
    {
        private readonly ApplicationDbContext _context;

        public ClienteRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Cliente>> GetAllAsync()
        {
            return await _context.Clientes
                .Where(c => c.Status)
                .ToListAsync();
        }

        public async Task<Cliente> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.Id == id && c.Status);
        }

        public async Task<Cliente> CreateAsync(Cliente cliente)
        {
            _context.Clientes.Add(cliente);
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<Cliente> UpdateAsync(Cliente cliente)
        {
            _context.Entry(cliente).State = EntityState.Modified;
            await _context.SaveChangesAsync();
            return cliente;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            var cliente = await _context.Clientes.FindAsync(id);
            if (cliente == null) return false;

            cliente.Status = false;
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> ExistsByRazonSocialRfcDireccionAsync(string razonSocial, string rfc, string? direccion)
        {
            return await _context.Clientes
                .AnyAsync(c => c.Status && 
                    c.RazonSocial == razonSocial && 
                    c.RFC == rfc && 
                    c.Direccion == direccion);
        }

        public async Task<int> GetIdEmpresaByUserIdAsync(int userId)
        {
            var usuario = await _context.Usuarios
                .FirstOrDefaultAsync(u => u.Id == userId);
            return usuario?.IdEmpresa ?? 0;
        }
    }
} 