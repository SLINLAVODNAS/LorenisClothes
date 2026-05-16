using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using LorenisClothes.Data;
using LorenisClothes.Extensions;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LorenisClothes.Controllers
{
    [AdminAuth]
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            ViewBag.TotalVentas = await _context.Pedidos.AnyAsync()
                ? await _context.Pedidos.SumAsync(p => p.Total)
                : 0;

            ViewBag.CantidadPedidos = await _context.Pedidos.CountAsync();
            ViewBag.TotalProductos = await _context.Productos.CountAsync();
            ViewBag.StockBajo = await _context.Productos.CountAsync(p => p.Stock < 5);

            var ultimosPedidos = await _context.Pedidos
                .OrderByDescending(p => p.FechaPedido)
                .Take(5)
                .ToListAsync();

            return View(ultimosPedidos);
        }
    }
}