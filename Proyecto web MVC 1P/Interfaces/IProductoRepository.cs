using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Interfaces
{
    public interface IProductoRepository : IRepository<Producto>
    {
        Task<IEnumerable<Producto>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Producto>> GetByCategoriaAsync(string categoria);
        Task<IEnumerable<Producto>> GetAvailableProductosAsync();
        Task<IEnumerable<Producto>> GetProductosWithUsuarioAsync();
        Task<Producto?> GetByIdWithUsuarioAsync(int id);
        Task<bool> HasComprasAsync(int productoId);
    }
}
