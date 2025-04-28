namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    namespace Proyecto_web_MVC_1P.Models
    {
        public class Compra
        {
            [Key]
            public int Id { get; set; }

            [Required]
            public int UsuarioId { get; set; }

            [ForeignKey("UsuarioId")]
            public Usuario? Usuario { get; set; }

            [Required]
            public int ProductoId { get; set; }

            [ForeignKey("ProductoId")]
            public Producto? Producto { get; set; }

            public DateTime FechaCompra { get; set; } = DateTime.Now;
        }
    }
}
