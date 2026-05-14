using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;
using LorenisClothes.Data;

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
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var pedidos = _context.Pedidos
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .ToList();

            return View(pedidos);
        }

        public IActionResult Details(int id)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var pedido = _context.Pedidos
                .Include(p => p.Detalles)
                .ThenInclude(d => d.Producto)
                .FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            return View(pedido);
        }

        [HttpPost]
        public IActionResult CambiarEstado(int id, string estado)
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            var pedido = _context.Pedidos.FirstOrDefault(p => p.Id == id);

            if (pedido == null)
            {
                return NotFound();
            }

            pedido.Estado = estado;

            _context.SaveChanges();

            return RedirectToAction("Details", new { id = pedido.Id });
        }
    }
}