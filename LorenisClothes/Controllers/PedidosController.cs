using LorenisClothes.Data;
using LorenisClothes.Extensions;
using LorenisClothes.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace LorenisClothes.Controllers
{
    [AdminAuth]
    public class PedidosController : Controller
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index(string estado, DateTime? inicio, DateTime? fin)
        {
            var consulta = _context.Pedidos.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(p => p.Estado == estado);
            }

            if (inicio.HasValue)
            {
                consulta = consulta.Where(p => p.FechaPedido >= inicio.Value);
            }

            if (fin.HasValue)
            {
                consulta = consulta.Where(p => p.FechaPedido <= fin.Value.AddDays(1).AddTicks(-1));
            }

            var pedidos = await consulta
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            ViewBag.Estados = new[] { "Pendiente", "Entregado", "Cancelado" };
            return View(pedidos);
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();

            var pedido = await _context.Pedidos
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }

        public async Task<IActionResult> Reporte(string estado, DateTime? inicio, DateTime? fin)
        {
            var consulta = _context.Pedidos.AsQueryable();

            if (!string.IsNullOrEmpty(estado))
            {
                consulta = consulta.Where(p => p.Estado == estado);
            }

            if (inicio.HasValue)
            {
                consulta = consulta.Where(p => p.FechaPedido >= inicio.Value);
            }

            if (fin.HasValue)
            {
                consulta = consulta.Where(p => p.FechaPedido <= fin.Value.AddDays(1).AddTicks(-1));
            }

            var pedidos = await consulta
                .OrderByDescending(p => p.FechaPedido)
                .ToListAsync();

            return View(pedidos);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CambiarEstado(int id, string nuevoEstado)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                pedido.Estado = nuevoEstado;
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var pedido = await _context.Pedidos.FindAsync(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}