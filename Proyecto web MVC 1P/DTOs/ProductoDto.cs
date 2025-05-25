using System.ComponentModel.DataAnnotations;

namespace Proyecto_web_MVC_1P.DTOs
{
    public class ProductoDto
    {
        public int Id { get; set; }
        public int UsuarioId { get; set; }
        public string NombreUsuario { get; set; }
        public string NombreProducto { get; set; }
        public string Descripcion { get; set; }
        public float Precio { get; set; }
        public string TipoProducto { get; set; }
        public string Categoria { get; set; }
        public string ImagenUrl { get; set; }
        public DateTime FechaPublicacion { get; set; }
        public bool Disponibilidad { get; set; }
    }

    public class CreateProductoDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre del producto no puede exceder 100 caracteres")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(240, ErrorMessage = "La descripción no puede exceder 240 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, float.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public float Precio { get; set; }

        [Required(ErrorMessage = "El tipo de producto es requerido")]
        [MaxLength(50, ErrorMessage = "El tipo de producto no puede exceder 50 caracteres")]
        public string TipoProducto { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [MaxLength(100, ErrorMessage = "La categoría no puede exceder 100 caracteres")]
        public string Categoria { get; set; }

        [MaxLength(240, ErrorMessage = "La URL de la imagen no puede exceder 240 caracteres")]
        [Url(ErrorMessage = "La URL de la imagen no tiene un formato válido")]
        public string ImagenUrl { get; set; }

        public bool Disponibilidad { get; set; } = true;
    }

    public class UpdateProductoDto
    {
        [Required(ErrorMessage = "El ID del usuario es requerido")]
        public int UsuarioId { get; set; }

        [Required(ErrorMessage = "El nombre del producto es requerido")]
        [MaxLength(100, ErrorMessage = "El nombre del producto no puede exceder 100 caracteres")]
        public string NombreProducto { get; set; }

        [Required(ErrorMessage = "La descripción es requerida")]
        [MaxLength(240, ErrorMessage = "La descripción no puede exceder 240 caracteres")]
        public string Descripcion { get; set; }

        [Required(ErrorMessage = "El precio es requerido")]
        [Range(0.01, float.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public float Precio { get; set; }

        [Required(ErrorMessage = "El tipo de producto es requerido")]
        [MaxLength(50, ErrorMessage = "El tipo de producto no puede exceder 50 caracteres")]
        public string TipoProducto { get; set; }

        [Required(ErrorMessage = "La categoría es requerida")]
        [MaxLength(100, ErrorMessage = "La categoría no puede exceder 100 caracteres")]
        public string Categoria { get; set; }

        [MaxLength(240, ErrorMessage = "La URL de la imagen no puede exceder 240 caracteres")]
        [Url(ErrorMessage = "La URL de la imagen no tiene un formato válido")]
        public string ImagenUrl { get; set; }

        public bool Disponibilidad { get; set; }
    }
}
