using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;
using Proyecto_web_MVC_1P.DTOs;

namespace Proyecto_web_MVC_1P.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosApiController : ControllerBase
    {
        private readonly ProyectoWebMVCP1Context _context;

        public ProductosApiController(ProyectoWebMVCP1Context context)
        {
            _context = context;
        }

        // GET: api/ProductosApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            var productos = await _context.Producto
                .Include(p => p.Usuario)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    NombreUsuario = p.Usuario.NombreUsuario,
                    NombreProducto = p.NombreProducto,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    TipoProducto = p.TipoProducto,
                    Categoria = p.Categoria,
                    ImagenUrl = p.ImagenUrl,
                    FechaPublicacion = p.FechaPublicacion,
                    Disponibilidad = p.Disponibilidad
                })
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/ProductosApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            var producto = await _context.Producto
                .Include(p => p.Usuario)
                .FirstOrDefaultAsync(p => p.Id == id);

            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            var productoDto = new ProductoDto
            {
                Id = producto.Id,
                UsuarioId = producto.UsuarioId,
                NombreUsuario = producto.Usuario.NombreUsuario,
                NombreProducto = producto.NombreProducto,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                TipoProducto = producto.TipoProducto,
                Categoria = producto.Categoria,
                ImagenUrl = producto.ImagenUrl,
                FechaPublicacion = producto.FechaPublicacion,
                Disponibilidad = producto.Disponibilidad
            };

            return Ok(productoDto);
        }

        // GET: api/ProductosApi/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductosByUsuario(int usuarioId)
        {
            var productos = await _context.Producto
                .Include(p => p.Usuario)
                .Where(p => p.UsuarioId == usuarioId)
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    NombreUsuario = p.Usuario.NombreUsuario,
                    NombreProducto = p.NombreProducto,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    TipoProducto = p.TipoProducto,
                    Categoria = p.Categoria,
                    ImagenUrl = p.ImagenUrl,
                    FechaPublicacion = p.FechaPublicacion,
                    Disponibilidad = p.Disponibilidad
                })
                .ToListAsync();

            return Ok(productos);
        }

        // GET: api/ProductosApi/categoria/{categoria}
        [HttpGet("categoria/{categoria}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductosByCategoria(string categoria)
        {
            var productos = await _context.Producto
                .Include(p => p.Usuario)
                .Where(p => p.Categoria.ToLower().Contains(categoria.ToLower()))
                .Select(p => new ProductoDto
                {
                    Id = p.Id,
                    UsuarioId = p.UsuarioId,
                    NombreUsuario = p.Usuario.NombreUsuario,
                    NombreProducto = p.NombreProducto,
                    Descripcion = p.Descripcion,
                    Precio = p.Precio,
                    TipoProducto = p.TipoProducto,
                    Categoria = p.Categoria,
                    ImagenUrl = p.ImagenUrl,
                    FechaPublicacion = p.FechaPublicacion,
                    Disponibilidad = p.Disponibilidad
                })
                .ToListAsync();

            return Ok(productos);
        }

        // POST: api/ProductosApi
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> CreateProducto(CreateProductoDto createProductoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar que el usuario existe
            if (!await _context.Usuario.AnyAsync(u => u.Id == createProductoDto.UsuarioId))
            {
                return BadRequest(new { message = "El usuario especificado no existe" });
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

            _context.Producto.Add(producto);
            await _context.SaveChangesAsync();

            // Cargar el usuario para la respuesta
            await _context.Entry(producto).Reference(p => p.Usuario).LoadAsync();

            var productoDto = new ProductoDto
            {
                Id = producto.Id,
                UsuarioId = producto.UsuarioId,
                NombreUsuario = producto.Usuario.NombreUsuario,
                NombreProducto = producto.NombreProducto,
                Descripcion = producto.Descripcion,
                Precio = producto.Precio,
                TipoProducto = producto.TipoProducto,
                Categoria = producto.Categoria,
                ImagenUrl = producto.ImagenUrl,
                FechaPublicacion = producto.FechaPublicacion,
                Disponibilidad = producto.Disponibilidad
            };

            return CreatedAtAction(nameof(GetProducto), new { id = producto.Id }, productoDto);
        }

        // PUT: api/ProductosApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, UpdateProductoDto updateProductoDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            // Verificar que el usuario existe si se está cambiando
            if (updateProductoDto.UsuarioId != producto.UsuarioId)
            {
                if (!await _context.Usuario.AnyAsync(u => u.Id == updateProductoDto.UsuarioId))
                {
                    return BadRequest(new { message = "El usuario especificado no existe" });
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

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ProductoExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/ProductosApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            var producto = await _context.Producto.FindAsync(id);
            if (producto == null)
            {
                return NotFound(new { message = "Producto no encontrado" });
            }

            // Verificar si el producto tiene compras asociadas
            var tieneCompras = await _context.Compra.AnyAsync(c => c.ProductoId == id);
            if (tieneCompras)
            {
                return BadRequest(new { message = "No se puede eliminar el producto porque tiene compras asociadas" });
            }

            _context.Producto.Remove(producto);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool ProductoExists(int id)
        {
            return _context.Producto.Any(e => e.Id == id);
        }
    }
}
