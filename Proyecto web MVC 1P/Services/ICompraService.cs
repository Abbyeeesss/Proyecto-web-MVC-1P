using Proyecto_web_MVC_1P.DTOs;

namespace Proyecto_web_MVC_1P.Services
{
    public interface ICompraService
    {
        Task<IEnumerable<CompraDto>> GetAllComprasAsync();
        Task<CompraDto?> GetCompraByIdAsync(int id);
        Task<IEnumerable<CompraDto>> GetComprasByUsuarioAsync(int usuarioId);
        Task<CompraDto> CreateCompraAsync(CreateCompraDto createCompraDto);
        Task<bool> UpdateCompraAsync(int id, UpdateCompraDto updateCompraDto);
        Task<bool> DeleteCompraAsync(int id);
        Task<EstadisticasComprasDto> GetEstadisticasAsync();
    }
}
