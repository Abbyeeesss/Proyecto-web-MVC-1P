namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Calificacion
    {
        [Key]
        public int IdCalificacion { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        public int IdUsuario { get; set; }

        [Required]
        [Range(1, 5)]
        public int Puntuacion { get; set; }

        public DateTime FechaCalificacion { get; set; } = DateTime.Now;
    }
}
