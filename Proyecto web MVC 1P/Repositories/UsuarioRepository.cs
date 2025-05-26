using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Interfaces;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Repositories
{
    public class UsuarioRepository : Repository<Usuario>, IUsuarioRepository
    {
        public UsuarioRepository(ProyectoWebMVCP1Context context) : base(context)
        {
        }

        public async Task<Usuario?> GetByEmailAsync(string email)
        {
            return await _dbSet.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email)
        {
            return await _dbSet.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> EmailExistsAsync(string email, int excludeId)
        {
            return await _dbSet.AnyAsync(u => u.Email == email && u.Id != excludeId);
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosWithProductosAsync()
        {
            return await _context.Usuario
                .Include(u => u.Productos)
                .ToListAsync();
        }

        public async Task<IEnumerable<Usuario>> GetUsuariosWithComprasAsync()
        {
            return await _context.Usuario
                .Include(u => u.Compras)
                .ToListAsync();
        }
    }
}
