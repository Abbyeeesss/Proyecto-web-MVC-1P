using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;

public class ProyectoWebMVCP1Context : DbContext
{
    public ProyectoWebMVCP1Context(DbContextOptions<ProyectoWebMVCP1Context> options)
        : base(options)
    {
    }

    public DbSet<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Usuario> Usuario { get; set; } = default!;
    public DbSet<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Producto> Producto { get; set; } = default!;
    public DbSet<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Compra> Compra { get; set; } = default!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración para SQLite
        modelBuilder.Entity<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Compra>()
            .HasOne(c => c.Producto)
            .WithMany()
            .HasForeignKey(c => c.ProductoId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Compra>()
            .HasOne(c => c.Usuario)
            .WithMany()
            .HasForeignKey(c => c.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models.Producto>()
            .HasOne(p => p.Usuario)
            .WithMany()
            .HasForeignKey(p => p.UsuarioId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}