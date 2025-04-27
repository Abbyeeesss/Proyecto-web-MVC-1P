namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Carrito
    {
        [Key]
        public int IdCarrito { get; set; }

        [Required]
        public int IdUsuario { get; set; } // FK a Usuario
    }

}
