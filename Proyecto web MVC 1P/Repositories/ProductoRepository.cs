using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Interfaces;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Repositories
{
    public class ProductoRepository : Repository<Producto>, IProductoRepository
    {
        public ProductoRepository(ProyectoWebMVCP1Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Producto>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Where(p => p.UsuarioId == usuarioId)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetByCategoriaAsync(string categoria)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Where(p => p.Categoria.ToLower().Contains(categoria.ToLower()))
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetAvailableProductosAsync()
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .Where(p => p.Disponibilidad)
                .ToListAsync();
        }

        public async Task<IEnumerable<Producto>> GetProductosWithUsuarioAsync()
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .ToListAsync();
        }

        public async Task<Producto?> GetByIdWithUsuarioAsync(int id)
        {
            return await _dbSet
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<bool> HasComprasAsync(int productoId)
        {
            return await _context.Compra.AnyAsync(c => c.ProductoId == productoId);
        }
    }
}
