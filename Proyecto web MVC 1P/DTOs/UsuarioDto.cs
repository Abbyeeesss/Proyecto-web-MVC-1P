using System.ComponentModel.DataAnnotations;

namespace Proyecto_web_MVC_1P.DTOs
{
    public class UsuarioDto
    {
        public int Id { get; set; }
        public string NombreUsuario { get; set; }
        public string Email { get; set; }
        public string Telefono { get; set; }
    }

    public class CreateUsuarioDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre de usuario no puede exceder 100 caracteres")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [MaxLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string Email { get; set; }

        [Required(ErrorMessage = "La contraseña es requerida")]
        [MaxLength(100, ErrorMessage = "La contraseña no puede exceder 100 caracteres")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string Contraseña { get; set; }

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string Telefono { get; set; }
    }

    public class UpdateUsuarioDto
    {
        [Required(ErrorMessage = "El nombre de usuario es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre de usuario no puede exceder 100 caracteres")]
        public string NombreUsuario { get; set; }

        [Required(ErrorMessage = "El email es requerido")]
        [EmailAddress(ErrorMessage = "El formato del email no es válido")]
        [MaxLength(100, ErrorMessage = "El email no puede exceder 100 caracteres")]
        public string Email { get; set; }

        [MaxLength(100, ErrorMessage = "La contraseña no puede exceder 100 caracteres")]
        [MinLength(6, ErrorMessage = "La contraseña debe tener al menos 6 caracteres")]
        public string? Contraseña { get; set; } // Opcional en actualización

        [Required(ErrorMessage = "El teléfono es requerido")]
        [Phone(ErrorMessage = "El formato del teléfono no es válido")]
        public string Telefono { get; set; }
    }
}
