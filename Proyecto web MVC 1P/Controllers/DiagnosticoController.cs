using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Controllers
{
    public class DiagnosticoController : Controller
    {
        private readonly ProyectoWebMVCP1Context _context;

        public DiagnosticoController(ProyectoWebMVCP1Context context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            var diagnostico = new
            {
                DatabasePath = _context.Database.GetConnectionString(),
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
                        Usuario = p.Usuario.NombreUsuario
                    })
                    .ToListAsync()
            };

            ViewBag.Diagnostico = diagnostico;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CrearProductoPrueba()
        {
            try
            {
                // Verificar si hay usuarios
                var usuario = await _context.Usuario.FirstOrDefaultAsync();
                if (usuario == null)
                {
                    // Crear un usuario de prueba
                    usuario = new Usuario
                    {
                        NombreUsuario = "Usuario Prueba",
                        Email = "prueba@test.com",
                        Contraseña = "123456",
                        Telefono = "1234567890"
                    };
                    _context.Usuario.Add(usuario);
                    await _context.SaveChangesAsync();
                }

                // Crear producto de prueba
                var producto = new Producto
                {
                    UsuarioId = usuario.Id,
                    NombreProducto = $"Producto Prueba {DateTime.Now:HH:mm:ss}",
                    Descripcion = "Producto creado desde diagnóstico",
                    Precio = 10.99f,
                    TipoProducto = "Prueba",
                    Categoria = "Test",
                    ImagenUrl = "/images/test.jpg",
                    FechaPublicacion = DateTime.Now,
                    Disponibilidad = true
                };

                _context.Producto.Add(producto);
                var result = await _context.SaveChangesAsync();

                TempData["Mensaje"] = $"Producto creado exitosamente. ID: {producto.Id}, Registros afectados: {result}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al crear producto: {ex.Message}";
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> LimpiarDatos()
        {
            try
            {
                _context.Compra.RemoveRange(_context.Compra);
                _context.Producto.RemoveRange(_context.Producto);
                _context.Usuario.RemoveRange(_context.Usuario);

                var result = await _context.SaveChangesAsync();
                TempData["Mensaje"] = $"Datos limpiados. Registros eliminados: {result}";
            }
            catch (Exception ex)
            {
                TempData["Error"] = $"Error al limpiar datos: {ex.Message}";
            }

            return RedirectToAction("Index");
        }
    }
}
