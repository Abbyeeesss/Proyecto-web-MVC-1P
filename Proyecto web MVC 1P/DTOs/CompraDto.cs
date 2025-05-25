using System.ComponentModel.DataAnnotations;

namespace Proyecto_web_MVC_1P.DTOs
{
    public class CompraDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public int ProductoId { get; set; }
        public string NombreProducto { get; set; }
        public float PrecioProducto { get; set; }
        public DateTime FechaCompra { get; set; }
    }

    public class CreateCompraDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El ID del producto es requerido")]
        public int ProductoId { get; set; }
    }

    public class UpdateCompraDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El ID del producto es requerido")]
        public int ProductoId { get; set; }

        [Required(ErrorMessage = "La fecha de compra es requerida")]
        public DateTime FechaCompra { get; set; }
    }

    public class EstadisticasComprasDto
    {
        public int TotalCompras { get; set; }
        public float TotalVentas { get; set; }
        public List<VentaMensualDto> VentasPorMes { get; set; } = new List<VentaMensualDto>();
    }

    public class VentaMensualDto
    {
        public int Año { get; set; }
        public int Mes { get; set; }
        public float TotalVentas { get; set; }
        public int CantidadCompras { get; set; }
    }
}
