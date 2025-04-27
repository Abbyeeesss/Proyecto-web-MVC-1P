namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Compra
    {
        [Key]
        public int IdCompra { get; set; }

        [Required]
        public int IdUsuarioComprador { get; set; }

        public DateTime FechaCompra { get; set; } = DateTime.Now;

        [Required]
        [Range(0, 999999)]
        public decimal TotalCompra { get; set; }
    }
}
