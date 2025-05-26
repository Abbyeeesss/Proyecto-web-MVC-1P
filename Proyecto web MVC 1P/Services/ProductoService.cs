using Proyecto_web_MVC_1P.DTOs;
using Proyecto_web_MVC_1P.Interfaces;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Services
{
    public class ProductoService : IProductoService
    {
        private readonly IProductoRepository _productoRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public ProductoService(IProductoRepository productoRepository, IUsuarioRepository usuarioRepository)
        {
            _productoRepository = productoRepository;
            _usuarioRepository = usuarioRepository;
        }

        public async Task<IEnumerable<ProductoDto>> GetAllProductosAsync()
        {
            var productos = await _productoRepository.GetProductosWithUsuarioAsync();
            return productos.Select(p => MapToDto(p));
        }

        public async Task<ProductoDto?> GetProductoByIdAsync(int id)
        {
            var producto = await _productoRepository.GetByIdWithUsuarioAsync(id);
            return producto != null ? MapToDto(producto) : null;
        }

        public async Task<IEnumerable<ProductoDto>> GetProductosByUsuarioAsync(int usuarioId)
        {
            var productos = await _productoRepository.GetByUsuarioIdAsync(usuarioId);
            return productos.Select(p => MapToDto(p));
        }

        public async Task<IEnumerable<ProductoDto>> GetProductosByCategoriaAsync(string categoria)
        {
            var productos = await _productoRepository.GetByCategoriaAsync(categoria);
            return productos.Select(p => MapToDto(p));
        }

        public async Task<ProductoDto> CreateProductoAsync(CreateProductoDto createProductoDto)
        {
            // Verificar que el usuario existe
            if (!await _usuarioRepository.ExistsAsync(createProductoDto.UsuarioId))
            {
                throw new InvalidOperationException("El usuario especificado no existe");
            }

            var producto = new Producto
            {
                UsuarioId = createProductoDto.UsuarioId,
                NombreProducto = createProductoDto.NombreProducto,
                Descripcion = createProductoDto.Descripcion,
                Precio = createProductoDto.Precio,
                TipoProducto = createProductoDto.TipoProducto,
                Categoria = createProductoDto.Categoria,
                ImagenUrl = createProductoDto.ImagenUrl,
                FechaPublicacion = DateTime.Now,
                Disponibilidad = createProductoDto.Disponibilidad
            };

            var createdProducto = await _productoRepository.AddAsync(producto);
            var productoWithUsuario = await _productoRepository.GetByIdWithUsuarioAsync(createdProducto.Id);
            
            return MapToDto(productoWithUsuario!);
        }

        public async Task<bool> UpdateProductoAsync(int id, UpdateProductoDto updateProductoDto)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null) return false;

            // Verificar que el usuario existe si se está cambiando
            if (updateProductoDto.UsuarioId != producto.UsuarioId)
            {
                if (!await _usuarioRepository.ExistsAsync(updateProductoDto.UsuarioId))
                {
                    throw new InvalidOperationException("El usuario especificado no existe");
                }
            }

            producto.UsuarioId = updateProductoDto.UsuarioId;
            producto.NombreProducto = updateProductoDto.NombreProducto;
            producto.Descripcion = updateProductoDto.Descripcion;
            producto.Precio = updateProductoDto.Precio;
            producto.TipoProducto = updateProductoDto.TipoProducto;
            producto.Categoria = updateProductoDto.Categoria;
            producto.ImagenUrl = updateProductoDto.ImagenUrl;
            producto.Disponibilidad = updateProductoDto.Disponibilidad;

            await _productoRepository.UpdateAsync(producto);
            return true;
        }

        public async Task<bool> DeleteProductoAsync(int id)
        {
            var producto = await _productoRepository.GetByIdAsync(id);
            if (producto == null) return false;

            // Verificar si el producto tiene compras asociadas
            if (await _productoRepository.HasComprasAsync(id))
            {
                throw new InvalidOperationException("No se puede eliminar el producto porque tiene compras asociadas");
            }

            await _productoRepository.DeleteAsync(producto);
            return true;
        }

        private static ProductoDto MapToDto(Producto producto)
        {
            return new ProductoDto
            {
                Id = producto.Id,
                UsuarioId = producto.UsuarioId,
                NombreUsuario = producto.Usuario?.NombreUsuario ?? "",
                NombreProducto = producto.NombreProducto,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                TipoProducto = producto.TipoProducto,
                Categoria = producto.Categoria,
                ImagenUrl = producto.ImagenUrl,
                FechaPublicacion = producto.FechaPublicacion,
                Disponibilidad = producto.Disponibilidad
            };
        }
    }
}
