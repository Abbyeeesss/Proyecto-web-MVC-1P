using Proyecto_web_MVC_1P.DTOs;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Services
{
    public interface IUsuarioService
    {
        Task<IEnumerable<UsuarioDto>> GetAllUsuariosAsync();
        Task<UsuarioDto?> GetUsuarioByIdAsync(int id);
        Task<UsuarioDto> CreateUsuarioAsync(CreateUsuarioDto createUsuarioDto);
        Task<bool> UpdateUsuarioAsync(int id, UpdateUsuarioDto updateUsuarioDto);
        Task<bool> DeleteUsuarioAsync(int id);
        Task<bool> EmailExistsAsync(string email, int? excludeId = null);
    }
}
