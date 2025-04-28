namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Proyecto_web_MVC_1P.Models
    {
        public class Producto
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public int UsuarioId { get; set; }

            [ForeignKey("UsuarioId")]
            public Usuario? Usuario { get; set; }

            [Required]
            [MaxLength(100)]
            public string NombreProducto { get; set; }

            [Required]
            [MaxLength(240)]
            public string Descripcion { get; set; }

            [Required]
            public float Precio { get; set; }

            [Required]
            [MaxLength(50)]
            public string TipoProducto { get; set; }

            [Required]
            [MaxLength(100)]
            public string Categoria { get; set; }

            [MaxLength(240)]
            public string ImagenUrl { get; set; }

            public DateTime FechaPublicacion { get; set; } = DateTime.Now;

            public bool Disponibilidad { get; set; }
        }
    }

}