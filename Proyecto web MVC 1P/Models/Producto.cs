namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Producto
    {
        [Key]
        public int IdProducto { get; set; }

        [Required]
        public int IdUsuario { get; set; } 

        [Required]
        [MaxLength(200)]
        public string NombreProducto { get; set; }

        [Required]
        [MaxLength(100)]
        public string Descripcion { get; set; }

        [Required]
        [Range(0, 99999)]
        public decimal Precio { get; set; }

        [Required]
        [MaxLength(50)]
        public string TipoProducto { get; set; } 

        [Required]
        [MaxLength(100)]
        public string Categoria { get; set; }

        [MaxLength(500)]
        public string? ImagenUrl { get; set; }

        public DateTime FechaPublicacion { get; set; } = DateTime.Now;

        public bool Disponibilidad { get; set; } = true;
    }
}