using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class DiagnosticoApiController : ControllerBase
    {
        private readonly ProyectoWebMVCP1Context _context;

        public DiagnosticoApiController(ProyectoWebMVCP1Context context)
        {
            _context = context;
        }

        [HttpGet("info")]
        public async Task<ActionResult> GetDatabaseInfo()
        {
            try
            {
                var info = new
                {
                    DatabasePath = _context.Database.GetConnectionString(),
                    CanConnect = await _context.Database.CanConnectAsync(),
                    TotalUsuarios = await _context.Usuario.CountAsync(),
                    TotalProductos = await _context.Producto.CountAsync(),
                    TotalCompras = await _context.Compra.CountAsync(),
                    UltimosProductos = await _context.Producto
                        .Include(p => p.Usuario)
                        .OrderByDescending(p => p.Id)
                        .Take(5)
                        .Select(p => new {
                            p.Id,
                            p.NombreProducto,
                            p.FechaPublicacion,
                            Usuario = p.Usuario.NombreUsuario,
                            p.Precio,
                            p.Disponibilidad
                        })
                        .ToListAsync(),
                    Timestamp = DateTime.Now
                };

                return Ok(info);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        [HttpPost("test-create")]
        public async Task<ActionResult> CreateTestProduct()
        {
            try
            {
                // Verificar si hay usuarios
                var usuario = await _context.Usuario.FirstOrDefaultAsync();
                if (usuario == null)
                {
                    return BadRequest(new { message = "No hay usuarios en la base de datos" });
                }

                // Crear producto de prueba
                var producto = new Producto
                {
                    UsuarioId = usuario.Id,
                    NombreProducto = $"API Test Product {DateTime.Now:HH:mm:ss}",
                    Descripcion = "Producto creado desde API de diagnóstico",
                    Precio = 15.99f,
                    TipoProducto = "Test API",
                    Categoria = "API Test",
                    ImagenUrl = "/images/api-test.jpg",
                    FechaPublicacion = DateTime.Now,
                    Disponibilidad = true
                };

                _context.Producto.Add(producto);
                var result = await _context.SaveChangesAsync();

                return Ok(new
                {
                    message = "Producto creado exitosamente desde API",
                    productId = producto.Id,
                    affectedRows = result,
                    producto = new
                    {
                        producto.Id,
                        producto.NombreProducto,
                        producto.FechaPublicacion,
                        Usuario = usuario.NombreUsuario
                    }
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}
