using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

    public class ProyectoWebMVCP1Context : DbContext
    {
        public ProyectoWebMVCP1Context (DbContextOptions<ProyectoWebMVCP1Context> options)
            : base(options)
        {
        }

        public DbSet<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Usuario> Usuario { get; set; } = default!;

public DbSet<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Producto> Producto { get; set; } = default!;

public DbSet<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Compra> Compra { get; set; } = default!;
    }
