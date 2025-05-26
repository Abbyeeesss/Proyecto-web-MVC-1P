using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Interfaces
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        Task<Usuario?> GetByEmailAsync(string email);
        Task<bool> EmailExistsAsync(string email);
        Task<bool> EmailExistsAsync(string email, int excludeId);
        Task<IEnumerable<Usuario>> GetUsuariosWithProductosAsync();
        Task<IEnumerable<Usuario>> GetUsuariosWithComprasAsync();
    }
}
