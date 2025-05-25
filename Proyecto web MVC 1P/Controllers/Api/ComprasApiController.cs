using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;
using Proyecto_web_MVC_1P.DTOs;

namespace Proyecto_web_MVC_1P.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ComprasApiController : ControllerBase
    {
        private readonly ProyectoWebMVCP1Context _context;

        public ComprasApiController(ProyectoWebMVCP1Context context)
        {
            _context = context;
        }

        // GET: api/ComprasApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<CompraDto>>> GetCompras()
        {
            var compras = await _context.Compra
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .Select(c => new CompraDto
                {
                    Id = c.Id,
                    UsuarioId = c.UsuarioId,
                    NombreUsuario = c.Usuario.NombreUsuario,
                    ProductoId = c.ProductoId,
                    NombreProducto = c.Producto.NombreProducto,
                    PrecioProducto = c.Producto.Precio,
                    FechaCompra = c.FechaCompra
                })
                .ToListAsync();

            return Ok(compras);
        }

        // GET: api/ComprasApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<CompraDto>> GetCompra(int id)
        {
            var compra = await _context.Compra
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);

            if (compra == null)
            {
                return NotFound(new { message = "Compra no encontrada" });
            }

            var compraDto = new CompraDto
            {
                Id = compra.Id,
                UsuarioId = compra.UsuarioId,
                NombreUsuario = compra.Usuario.NombreUsuario,
                ProductoId = compra.ProductoId,
                NombreProducto = compra.Producto.NombreProducto,
                PrecioProducto = compra.Producto.Precio,
                FechaCompra = compra.FechaCompra
            };

            return Ok(compraDto);
        }

        // GET: api/ComprasApi/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<CompraDto>>> GetComprasByUsuario(int usuarioId)
        {
            var compras = await _context.Compra
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .Where(c => c.UsuarioId == usuarioId)
                .Select(c => new CompraDto
                {
                    Id = c.Id,
                    UsuarioId = c.UsuarioId,
                    NombreUsuario = c.Usuario.NombreUsuario,
                    ProductoId = c.ProductoId,
                    NombreProducto = c.Producto.NombreProducto,
                    PrecioProducto = c.Producto.Precio,
                    FechaCompra = c.FechaCompra
                })
                .ToListAsync();

            return Ok(compras);
        }

        // GET: api/ComprasApi/estadisticas
        [HttpGet("estadisticas")]
        public async Task<ActionResult<EstadisticasComprasDto>> GetEstadisticasCompras()
        {
            var totalCompras = await _context.Compra.CountAsync();
            var totalVentas = await _context.Compra
                .Include(c => c.Producto)
                .SumAsync(c => c.Producto.Precio);

            var ventasPorMes = await _context.Compra
                .Include(c => c.Producto)
                .GroupBy(c => new { c.FechaCompra.Year, c.FechaCompra.Month })
                .Select(g => new VentaMensualDto
                {
                    Año = g.Key.Year,
                    Mes = g.Key.Month,
                    TotalVentas = g.Sum(c => c.Producto.Precio),
                    CantidadCompras = g.Count()
                })
                .OrderByDescending(v => v.Año)
                .ThenByDescending(v => v.Mes)
                .Take(12)
                .ToListAsync();

            var estadisticas = new EstadisticasComprasDto
            {
                TotalCompras = totalCompras,
                TotalVentas = totalVentas,
                VentasPorMes = ventasPorMes
            };

            return Ok(estadisticas);
        }

        // POST: api/ComprasApi
        [HttpPost]
        public async Task<ActionResult<CompraDto>> CreateCompra(CreateCompraDto createCompraDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar que el usuario existe
            if (!await _context.Usuario.AnyAsync(u => u.Id == createCompraDto.UsuarioId))
            {
                return BadRequest(new { message = "El usuario especificado no existe" });
            }

            // Verificar que el producto existe y está disponible
            var producto = await _context.Producto.FindAsync(createCompraDto.ProductoId);
            if (producto == null)
            {
                return BadRequest(new { message = "El producto especificado no existe" });
            }

            if (!producto.Disponibilidad)
            {
                return BadRequest(new { message = "El producto no está disponible para compra" });
            }

            var compra = new Compra
            {
                UsuarioId = createCompraDto.UsuarioId,
                ProductoId = createCompraDto.ProductoId,
                FechaCompra = DateTime.Now
            };

            _context.Compra.Add(compra);
            await _context.SaveChangesAsync();

            // Cargar las entidades relacionadas para la respuesta
            await _context.Entry(compra).Reference(c => c.Usuario).LoadAsync();
            await _context.Entry(compra).Reference(c => c.Producto).LoadAsync();

            var compraDto = new CompraDto
            {
                Id = compra.Id,
                UsuarioId = compra.UsuarioId,
                NombreUsuario = compra.Usuario.NombreUsuario,
                ProductoId = compra.ProductoId,
                NombreProducto = compra.Producto.NombreProducto,
                PrecioProducto = compra.Producto.Precio,
                FechaCompra = compra.FechaCompra
            };

            return CreatedAtAction(nameof(GetCompra), new { id = compra.Id }, compraDto);
        }

        // PUT: api/ComprasApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCompra(int id, UpdateCompraDto updateCompraDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound(new { message = "Compra no encontrada" });
            }

            compra.UsuarioId = updateCompraDto.UsuarioId;
            compra.ProductoId = updateCompraDto.ProductoId;
            compra.FechaCompra = updateCompraDto.FechaCompra;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CompraExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/ComprasApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCompra(int id)
        {
            var compra = await _context.Compra.FindAsync(id);
            if (compra == null)
            {
                return NotFound(new { message = "Compra no encontrada" });
            }

            _context.Compra.Remove(compra);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool CompraExists(int id)
        {
            return _context.Compra.Any(e => e.Id == id);
        }
    }
}
