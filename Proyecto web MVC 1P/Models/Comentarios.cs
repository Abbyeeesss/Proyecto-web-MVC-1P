namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CompraDetalle
    {
        [Key]
        public int IdCompraDetalle { get; set; }

        [Required]
        public int IdCompra { get; set; }

        [Required]
        public int IdProducto { get; set; }

        [Required]
        [Range(1, 9999)]
        public int Cantidad { get; set; }

        [Required]
        [Range(0, 999999)]
        public decimal PrecioUnitario { get; set; }
    }
}
