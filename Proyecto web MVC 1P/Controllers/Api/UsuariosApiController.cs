using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;
using Proyecto_web_MVC_1P.DTOs;

namespace Proyecto_web_MVC_1P.Controllers.Api
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosApiController : ControllerBase
    {
        private readonly ProyectoWebMVCP1Context _context;

        public UsuariosApiController(ProyectoWebMVCP1Context context)
        {
            _context = context;
        }

        // GET: api/UsuariosApi
        [HttpGet]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> GetUsuarios()
        {
            var usuarios = await _context.Usuario
                .Select(u => new UsuarioDto
                {
                    Id = u.Id,
                    NombreUsuario = u.NombreUsuario,
                    Email = u.Email,
                    Telefono = u.Telefono
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        // GET: api/UsuariosApi/5
        [HttpGet("{id}")]
        public async Task<ActionResult<UsuarioDto>> GetUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);

            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Telefono = usuario.Telefono
            };

            return Ok(usuarioDto);
        }

        // POST: api/UsuariosApi
        [HttpPost]
        public async Task<ActionResult<UsuarioDto>> CreateUsuario(CreateUsuarioDto createUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Verificar si el email ya existe
            if (await _context.Usuario.AnyAsync(u => u.Email == createUsuarioDto.Email))
            {
                return BadRequest(new { message = "El email ya está registrado" });
            }

            var usuario = new Usuario
            {
                NombreUsuario = createUsuarioDto.NombreUsuario,
                Email = createUsuarioDto.Email,
                Contraseña = createUsuarioDto.Contraseña,
                Telefono = createUsuarioDto.Telefono
            };

            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();

            var usuarioDto = new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Telefono = usuario.Telefono
            };

            return CreatedAtAction(nameof(GetUsuario), new { id = usuario.Id }, usuarioDto);
        }

        // PUT: api/UsuariosApi/5
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUsuario(int id, UpdateUsuarioDto updateUsuarioDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            // Verificar si el email ya existe en otro usuario
            if (await _context.Usuario.AnyAsync(u => u.Email == updateUsuarioDto.Email && u.Id != id))
            {
                return BadRequest(new { message = "El email ya está registrado por otro usuario" });
            }

            usuario.NombreUsuario = updateUsuarioDto.NombreUsuario;
            usuario.Email = updateUsuarioDto.Email;
            usuario.Telefono = updateUsuarioDto.Telefono;

            if (!string.IsNullOrEmpty(updateUsuarioDto.Contraseña))
            {
                usuario.Contraseña = updateUsuarioDto.Contraseña;
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                throw;
            }

            return NoContent();
        }

        // DELETE: api/UsuariosApi/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }

            // Verificar si el usuario tiene productos o compras asociadas
            var tieneProductos = await _context.Producto.AnyAsync(p => p.UsuarioId == id);
            var tieneCompras = await _context.Compra.AnyAsync(c => c.UsuarioId == id);

            if (tieneProductos || tieneCompras)
            {
                return BadRequest(new { message = "No se puede eliminar el usuario porque tiene productos o compras asociadas" });
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}
