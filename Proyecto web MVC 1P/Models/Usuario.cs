namespace Proyecto_web_MVC_1P.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Usuario
    {
        [Key]
        public int IdUsuario { get; set; }

        [Required]
        [MaxLength(100)]
        public string Nombre { get; set; }

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        public string Correo { get; set; }

        [Required]
        [MaxLength(100)]
        public string Contraseña { get; set; }

        [MaxLength(500)]
        public string? Descripcion { get; set; }

        [MaxLength(250)]
        public string? RedesSociales { get; set; }

        public bool EsVendedor { get; set; }
        public bool EsComprador { get; set; }
    }
}