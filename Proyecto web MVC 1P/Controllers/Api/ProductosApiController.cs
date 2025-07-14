using Microsoft.AspNetCore.Mvc;
using Proyecto_web_MVC_1P.DTOs;
using Proyecto_web_MVC_1P.Services;
using Microsoft.AspNetCore.Http; // Necesario para IFormFile

namespace Proyecto_web_MVC_1P.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductosApiController : ControllerBase
    {
        private readonly IProductoService _productoService;
        private readonly IFileService _fileService;
        private readonly IUsuarioService _usuarioService; // Inyectar IUsuarioService para validaciones

        public ProductosApiController(
            IProductoService productoService,
            IFileService fileService,
            IUsuarioService usuarioService) // Añadir al constructor
        {
            _productoService = productoService;
            _fileService = fileService;
            _usuarioService = usuarioService; // Asignar
        }

        // GET: api/ProductosApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductos()
        {
            try
            {
                var productos = await _productoService.GetAllProductosAsync();
                // --- LÍNEA CLAVE PARA DIAGNÓSTICO ---
                Console.WriteLine($"[ProductosApiController] Se recuperaron {productos.Count()} productos del servicio.");
                // ---------------------------------------
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al obtener productos", details = ex.Message });
            }
        }

        // GET: api/ProductosApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<ProductoDto>> GetProducto(int id)
        {
            try
            {
                var producto = await _productoService.GetProductoByIdAsync(id);
                if (producto == null)
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }
                return Ok(producto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al obtener producto por ID", details = ex.Message });
            }
        }

        // GET: api/ProductosApi/usuario/5
        [HttpGet("usuario/{usuarioId}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductosByUsuario(int usuarioId)
        {
            try
            {
                var productos = await _productoService.GetProductosByUsuarioAsync(usuarioId);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al obtener productos por usuario", details = ex.Message });
            }
        }

        // GET: api/ProductosApi/categoria/{categoria}
        [HttpGet("categoria/{categoria}")]
        public async Task<ActionResult<IEnumerable<ProductoDto>>> GetProductosByCategoria(string categoria)
        {
            try
            {
                var productos = await _productoService.GetProductosByCategoriaAsync(categoria);
                return Ok(productos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al obtener productos por categoría", details = ex.Message });
            }
        }

        // POST: api/ProductosApi
        [HttpPost]
        public async Task<ActionResult<ProductoDto>> CreateProducto([FromForm] CreateProductoDto createProductoDto, IFormFile? imagen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Validar que el usuario existe usando el servicio de usuario
                if (!await _usuarioService.EmailExistsAsync(createProductoDto.UsuarioId.ToString())) // Asumiendo que UsuarioId es un email o similar para esta validación
                {
                    // Si UsuarioId es un int, deberías usar _usuarioService.GetUsuarioByIdAsync(createProductoDto.UsuarioId)
                    // y verificar si es null, o _usuarioService.ExistsAsync(createProductoDto.UsuarioId) si existe ese método.
                    // Por ahora, lo dejo como un ejemplo de validación.
                    // Si tu UsuarioId es un int, la validación correcta sería:
                    if (await _usuarioService.GetUsuarioByIdAsync(createProductoDto.UsuarioId) == null)
                    {
                        return BadRequest(new { message = "El usuario especificado no existe." });
                    }
                }

                string? imagenUrl = null;
                if (imagen != null)
                {
                    try
                    {
                        imagenUrl = await _fileService.SaveFileAsync(imagen, "productos");
                    }
                    catch (ArgumentException ex)
                    {
                        return BadRequest(new { message = ex.Message });
                    }
                }

                // Asignar la URL de la imagen al DTO antes de pasarlo al servicio
                // Si imagenUrl es null, usa la que viene en el DTO (si la hay) o una por defecto
                createProductoDto.ImagenUrl = imagenUrl ?? createProductoDto.ImagenUrl ?? "/images/no-image.png";

                var productoDto = await _productoService.CreateProductoAsync(createProductoDto);
                return CreatedAtAction(nameof(GetProducto), new { id = productoDto.Id }, productoDto);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al crear producto", details = ex.Message });
            }
        }

        // PUT: api/ProductosApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateProducto(int id, [FromForm] UpdateProductoDto updateProductoDto, IFormFile? imagen)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            try
            {
                // Validar que el usuario existe si se está cambiando
                var existingProduct = await _productoService.GetProductoByIdAsync(id);
                if (existingProduct == null)
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }

                if (updateProductoDto.UsuarioId != existingProduct.UsuarioId)
                {
                    if (await _usuarioService.GetUsuarioByIdAsync(updateProductoDto.UsuarioId) == null)
                    {
                        return BadRequest(new { message = "El usuario especificado no existe." });
                    }
                }

                // Manejar nueva imagen
                if (imagen != null)
                {
                    // Eliminar imagen anterior si existe
                    if (!string.IsNullOrEmpty(existingProduct.ImagenUrl) && existingProduct.ImagenUrl != "/images/no-image.png")
                    {
                        await _fileService.DeleteFileAsync(existingProduct.ImagenUrl);
                    }

                    // Guardar nueva imagen
                    updateProductoDto.ImagenUrl = await _fileService.SaveFileAsync(imagen, "productos");
                }
                else if (string.IsNullOrEmpty(updateProductoDto.ImagenUrl) && !string.IsNullOrEmpty(existingProduct.ImagenUrl))
                {
                    // Si la URL de la imagen se vacía en el DTO y había una imagen, eliminarla
                    if (existingProduct.ImagenUrl != "/images/no-image.png")
                    {
                        await _fileService.DeleteFileAsync(existingProduct.ImagenUrl);
                    }
                    updateProductoDto.ImagenUrl = "/images/no-image.png"; // O dejarla vacía si prefieres
                }
                else if (string.IsNullOrEmpty(updateProductoDto.ImagenUrl))
                {
                    // Si no se proporciona nueva imagen y la URL en el DTO es vacía, usar la por defecto
                    updateProductoDto.ImagenUrl = "/images/no-image.png";
                }


                var result = await _productoService.UpdateProductoAsync(id, updateProductoDto);
                if (!result)
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al actualizar producto", details = ex.Message });
            }
        }

        // DELETE: api/ProductosApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProducto(int id)
        {
            try
            {
                var productoToDelete = await _productoService.GetProductoByIdAsync(id);
                if (productoToDelete == null)
                {
                    return NotFound(new { message = "Producto no encontrado" });
                }

                // Eliminar imagen asociada antes de eliminar el producto
                if (!string.IsNullOrEmpty(productoToDelete.ImagenUrl) && productoToDelete.ImagenUrl != "/images/no-image.png")
                {
                    await _fileService.DeleteFileAsync(productoToDelete.ImagenUrl);
                }

                var result = await _productoService.DeleteProductoAsync(id);
                if (!result)
                {
                    return BadRequest(new { message = "No se pudo eliminar el producto (posiblemente tiene compras asociadas)" });
                }
                return NoContent();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { message = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { message = "Error interno del servidor al eliminar producto", details = ex.Message });
            }
        }
    }
}
