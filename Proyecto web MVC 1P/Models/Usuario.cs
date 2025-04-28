namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    namespace Proyecto_web_MVC_1P.Models
    {
        public class Usuario
        {
            [Key]
            public int Id { get; set; }

            [Required]
            [MaxLength(100)]
            public string NombreUsuario { get; set; }

            [Required]
            [EmailAddress]
            [MaxLength(100)]
            public string Email { get; set; }

            [Required]
            [MaxLength(100)]
            public string Contraseña { get; set; }
            
            [Required]
            [Phone]
            public string Telefono { get; set; }
        }
    }
}