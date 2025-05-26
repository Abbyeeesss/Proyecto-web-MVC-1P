using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;
using Proyecto_web_MVC_1P.DTOs;

namespace Proyecto_web_MVC_1P.Interfaces
{
    public interface ICompraRepository : IRepository<Compra>
    {
        Task<IEnumerable<Compra>> GetByUsuarioIdAsync(int usuarioId);
        Task<IEnumerable<Compra>> GetByProductoIdAsync(int productoId);
        Task<IEnumerable<Compra>> GetComprasWithDetailsAsync();
        Task<Compra?> GetByIdWithDetailsAsync(int id);
        Task<EstadisticasComprasDto> GetEstadisticasAsync();
        Task<IEnumerable<VentaMensualDto>> GetVentasPorMesAsync(int meses = 12);
        Task<float> GetTotalVentasAsync();
        Task<int> GetTotalComprasAsync();
    }
}
