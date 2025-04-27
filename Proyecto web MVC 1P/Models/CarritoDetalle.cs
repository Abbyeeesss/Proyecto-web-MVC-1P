namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CarritoDetalle
    {
        [Key]
        public int IdDetalle { get; set; }

        [Required]
        public int IdCarrito { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        [Range(1, 9999)]
        public int Cantidad { get; set; }
    }

}
