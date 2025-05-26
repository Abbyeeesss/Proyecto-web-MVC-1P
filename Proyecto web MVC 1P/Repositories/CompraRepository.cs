using Microsoft.EntityFrameworkCore;
using Proyecto_web_MVC_1P.Interfaces;
using Proyecto_web_MVC_1P.Models.Proyecto_web_MVC_1P.Models;
using Proyecto_web_MVC_1P.DTOs;

namespace Proyecto_web_MVC_1P.Repositories
{
    public class CompraRepository : Repository<Compra>, ICompraRepository
    {
        public CompraRepository(ProyectoWebMVCP1Context context) : base(context)
        {
        }

        public async Task<IEnumerable<Compra>> GetByUsuarioIdAsync(int usuarioId)
        {
            return await _dbSet
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .Where(c => c.UsuarioId == usuarioId)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<IEnumerable<Compra>> GetByProductoIdAsync(int productoId)
        {
            return await _dbSet
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .Where(c => c.ProductoId == productoId)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<IEnumerable<Compra>> GetComprasWithDetailsAsync()
        {
            return await _dbSet
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .OrderByDescending(c => c.FechaCompra)
                .ToListAsync();
        }

        public async Task<Compra?> GetByIdWithDetailsAsync(int id)
        {
            return await _dbSet
                .Include(c => c.Usuario)
                .Include(c => c.Producto)
                .FirstOrDefaultAsync(c => c.Id == id);
        }

        public async Task<EstadisticasComprasDto> GetEstadisticasAsync()
        {
            var totalCompras = await GetTotalComprasAsync();
            var totalVentas = await GetTotalVentasAsync();
            var ventasPorMes = await GetVentasPorMesAsync();

            return new EstadisticasComprasDto
            {
                TotalCompras = totalCompras,
                TotalVentas = totalVentas,
                VentasPorMes = ventasPorMes.ToList()
            };
        }

        public async Task<IEnumerable<VentaMensualDto>> GetVentasPorMesAsync(int meses = 12)
        {
            return await _dbSet
                .Include(c => c.Producto)
                .GroupBy(c => new { c.FechaCompra.Year, c.FechaCompra.Month })
                .Select(g => new VentaMensualDto
                {
                    Año = g.Key.Year,
                    Mes = g.Key.Month,
                    TotalVentas = g.Sum(c => c.Producto.Precio),
                    CantidadCompras = g.Count()
                })
                .OrderByDescending(v => v.Año)
                .ThenByDescending(v => v.Mes)
                .Take(meses)
                .ToListAsync();
        }

        public async Task<float> GetTotalVentasAsync()
        {
            return await _dbSet
                .Include(c => c.Producto)
                .SumAsync(c => c.Producto.Precio);
        }

        public async Task<int> GetTotalComprasAsync()
        {
            return await _dbSet.CountAsync();
        }
    }
}
