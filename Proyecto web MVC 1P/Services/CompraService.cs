using Proyecto_web_MVC_1P.DTOs;
using Proyecto_web_MVC_1P.Interfaces;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Services
{
    public class CompraService : ICompraService
    {
        private readonly ICompraRepository _compraRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProductoRepository _productoRepository;

        public CompraService(
            ICompraRepository compraRepository,
            IUsuarioRepository usuarioRepository,
            IProductoRepository productoRepository)
        {
            _compraRepository = compraRepository;
            _usuarioRepository = usuarioRepository;
            _productoRepository = productoRepository;
        }

        public async Task<IEnumerable<CompraDto>> GetAllComprasAsync()
        {
            var compras = await _compraRepository.GetComprasWithDetailsAsync();
            return compras.Select(c => MapToDto(c));
        }

        public async Task<CompraDto?> GetCompraByIdAsync(int id)
        {
            var compra = await _compraRepository.GetByIdWithDetailsAsync(id);
            return compra != null ? MapToDto(compra) : null;
        }

        public async Task<IEnumerable<CompraDto>> GetComprasByUsuarioAsync(int usuarioId)
        {
            var compras = await _compraRepository.GetByUsuarioIdAsync(usuarioId);
            return compras.Select(c => MapToDto(c));
        }

        public async Task<CompraDto> CreateCompraAsync(CreateCompraDto createCompraDto)
        {
            // Verificar que el usuario existe
            if (!await _usuarioRepository.ExistsAsync(createCompraDto.UsuarioId))
            {
                throw new InvalidOperationException("El usuario especificado no existe");
            }

            // Verificar que el producto existe y está disponible
            var producto = await _productoRepository.GetByIdAsync(createCompraDto.ProductoId);
            if (producto == null)
            {
                throw new InvalidOperationException("El producto especificado no existe");
            }

            if (!producto.Disponibilidad)
            {
                throw new InvalidOperationException("El producto no está disponible para compra");
            }

            var compra = new Compra
            {
                UsuarioId = createCompraDto.UsuarioId,
                ProductoId = createCompraDto.ProductoId,
                FechaCompra = DateTime.Now
            };

            var createdCompra = await _compraRepository.AddAsync(compra);
            var compraWithDetails = await _compraRepository.GetByIdWithDetailsAsync(createdCompra.Id);

            return MapToDto(compraWithDetails!);
        }

        public async Task<bool> UpdateCompraAsync(int id, UpdateCompraDto updateCompraDto)
        {
            var compra = await _compraRepository.GetByIdAsync(id);
            if (compra == null) return false;

            // Verificar que el usuario y producto existen
            if (!await _usuarioRepository.ExistsAsync(updateCompraDto.UsuarioId))
            {
                throw new InvalidOperationException("El usuario especificado no existe");
            }

            if (!await _productoRepository.ExistsAsync(updateCompraDto.ProductoId))
            {
                throw new InvalidOperationException("El producto especificado no existe");
            }

            compra.UsuarioId = updateCompraDto.UsuarioId;
            compra.ProductoId = updateCompraDto.ProductoId;
            compra.FechaCompra = updateCompraDto.FechaCompra;

            await _compraRepository.UpdateAsync(compra);
            return true;
        }

        public async Task<bool> DeleteCompraAsync(int id)
        {
            var compra = await _compraRepository.GetByIdAsync(id);
            if (compra == null) return false;

            await _compraRepository.DeleteAsync(compra);
            return true;
        }

        public async Task<EstadisticasComprasDto> GetEstadisticasAsync()
        {
            return await _compraRepository.GetEstadisticasAsync();
        }

        private static CompraDto MapToDto(Compra compra)
        {
            return new CompraDto
            {
                Id = compra.Id,
                UsuarioId = compra.UsuarioId,
                NombreUsuario = compra.Usuario?.NombreUsuario ?? "",
                ProductoId = compra.ProductoId,
                NombreProducto = compra.Producto?.NombreProducto ?? "",
                PrecioProducto = compra.Producto?.Precio ?? 0,
                FechaCompra = compra.FechaCompra
            };
        }
    }
}
