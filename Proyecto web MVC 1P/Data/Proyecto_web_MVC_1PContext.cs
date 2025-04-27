using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Models;

namespace Proyecto_web_MVC_1P.Data
{
    public class Proyecto_web_MVC_1PContext : DbContext
    {
        public Proyecto_web_MVC_1PContext (DbContextOptions<Proyecto_web_MVC_1PContext> options)
            : base(options)
        {
        }

        public DbSet<Proyecto_web_MVC_1P.Models.Usuario> Usuario { get; set; } = default!;
        public DbSet<Proyecto_web_MVC_1P.Models.Producto> Producto { get; set; } = default!;
        public DbSet<Proyecto_web_MVC_1P.Models.Carrito> Carrito { get; set; } = default!;
        public DbSet<Proyecto_web_MVC_1P.Models.Compra> Compra { get; set; } = default!;
        public DbSet<Proyecto_web_MVC_1P.Models.Calificacion> Calificacion { get; set; } = default!;
        public DbSet<Proyecto_web_MVC_1P.Models.Notificacion> Notificacion { get; set; } = default!;
    }
}
