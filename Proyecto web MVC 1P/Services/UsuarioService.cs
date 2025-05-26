using Proyecto_web_MVC_1P.DTOs;
using Proyecto_web_MVC_1P.Interfaces;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Services
{
    public class UsuarioService : IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IProductoRepository _productoRepository;
        private readonly ICompraRepository _compraRepository;

        public UsuarioService(
            IUsuarioRepository usuarioRepository,
            IProductoRepository productoRepository,
            ICompraRepository compraRepository)
        {
            _usuarioRepository = usuarioRepository;
            _productoRepository = productoRepository;
            _compraRepository = compraRepository;
        }

        public async Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync()
        {
            var usuarios = await _usuarioRepository.GetAllAsync();
            return usuarios.Select(u => new UsuarioDto
            {
                Id = u.Id,
                NombreUsuario = u.NombreUsuario,
                Email = u.Email,
                Telefono = u.Telefono
            });
        }

        public async Task<UsuarioDto?> GetUsuarioByIdAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return null;

            return new UsuarioDto
            {
                Id = usuario.Id,
                NombreUsuario = usuario.NombreUsuario,
                Email = usuario.Email,
                Telefono = usuario.Telefono
            };
        }

        public async Task<UsuarioDto> CreateUsuarioAsync(CreateUsuarioDto createUsuarioDto)
        {
            // Validar que el email no exista
            if (await _usuarioRepository.EmailExistsAsync(createUsuarioDto.Email))
            {
                throw new InvalidOperationException("El email ya está registrado");
            }

            var usuario = new Usuario
            {
                NombreUsuario = createUsuarioDto.NombreUsuario,
                Email = createUsuarioDto.Email,
                Contraseña = createUsuarioDto.Contraseña, // En producción, hashear la contraseña
                Telefono = createUsuarioDto.Telefono
            };

            var createdUsuario = await _usuarioRepository.AddAsync(usuario);

            return new UsuarioDto
            {
                Id = createdUsuario.Id,
                NombreUsuario = createdUsuario.NombreUsuario,
                Email = createdUsuario.Email,
                Telefono = createdUsuario.Telefono
            };
        }

        public async Task<bool> UpdateUsuarioAsync(int id, UpdateUsuarioDto updateUsuarioDto)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return false;

            // Validar que el email no exista en otro usuario
            if (await _usuarioRepository.EmailExistsAsync(updateUsuarioDto.Email, id))
            {
                throw new InvalidOperationException("El email ya está registrado por otro usuario");
            }

            usuario.NombreUsuario = updateUsuarioDto.NombreUsuario;
            usuario.Email = updateUsuarioDto.Email;
            usuario.Telefono = updateUsuarioDto.Telefono;

            if (!string.IsNullOrEmpty(updateUsuarioDto.Contraseña))
            {
                usuario.Contraseña = updateUsuarioDto.Contraseña; // En producción, hashear la contraseña
            }

            await _usuarioRepository.UpdateAsync(usuario);
            return true;
        }

        public async Task<bool> DeleteUsuarioAsync(int id)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(id);
            if (usuario == null) return false;

            // Verificar si el usuario tiene productos o compras asociadas
            var tieneProductos = (await _productoRepository.GetByUsuarioIdAsync(id)).Any();
            var tieneCompras = (await _compraRepository.GetByUsuarioIdAsync(id)).Any();

            if (tieneProductos || tieneCompras)
            {
                throw new InvalidOperationException("No se puede eliminar el usuario porque tiene productos o compras asociadas");
            }

            await _usuarioRepository.DeleteAsync(usuario);
            return true;
        }

        public async Task<bool> EmailExistsAsync(string email, int? excludeId = null)
        {
            if (excludeId.HasValue)
            {
                return await _usuarioRepository.EmailExistsAsync(email, excludeId.Value);
            }
            return await _usuarioRepository.EmailExistsAsync(email);
        }
    }
}
