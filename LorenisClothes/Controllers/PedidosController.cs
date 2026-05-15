using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LorenisClothes.Data;
using System.Linq;

namespace LorenisClothes.Controllers
{
    public class PedidosController : Controller
    {
        private readonly AppDbContext _context;

        public PedidosController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            var pedidos = _context.Pedidos
                .OrderByDescending(p => p.FechaPedido)
                .ToList();

            return View(pedidos);
        }

        public IActionResult Details(int id)
        {
            var pedido = _context.Pedidos
                .Include(p => p.Detalles)
                    .ThenInclude(d => d.Producto)
                .FirstOrDefault(p => p.Id == id);

            if (pedido == null) return NotFound();

            return View(pedido);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Eliminar(int id)
        {
            var pedido = _context.Pedidos.Find(id);
            if (pedido != null)
            {
                _context.Pedidos.Remove(pedido);
                _context.SaveChanges();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}