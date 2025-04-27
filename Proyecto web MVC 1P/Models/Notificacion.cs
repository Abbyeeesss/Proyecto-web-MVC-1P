namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Notificacion
    {
        [Key]
        public int IdNotificacion { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Titulo { get; set; }

        [Required]
        [MaxLength(500)]
        public string Mensaje { get; set; }

        public DateTime FechaEnvio { get; set; } = DateTime.Now;

        public bool Estado { get; set; } = false; 
    }
}
