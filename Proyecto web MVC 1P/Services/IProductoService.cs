using Proyecto_web_MVC_1P.DTOs;

namespace Proyecto_web_MVC_1P.Services
{
    public interface IProductoService
    {
        Task<IEnumerable<ProductoDto>> GetAllProductosAsync();
        Task<ProductoDto?> GetProductoByIdAsync(int id);
        Task<IEnumerable<ProductoDto>> GetProductosByUsuarioAsync(int usuarioId);
        Task<IEnumerable<ProductoDto>> GetProductosByCategoriaAsync(string categoria);
        Task<ProductoDto> CreateProductoAsync(CreateProductoDto createProductoDto);
        Task<bool> UpdateProductoAsync(int id, UpdateProductoDto updateProductoDto);
        Task<bool> DeleteProductoAsync(int id);
    }
}
