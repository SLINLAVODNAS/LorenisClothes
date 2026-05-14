using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LorenisClothes.Data;
using System.Linq;

namespace LorenisClothes.Controllers
{
    public class DashboardController : Controller
    {
        private readonly AppDbContext _context;

        public DashboardController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            if (HttpContext.Session.GetString("Admin") == null)
            {
                return RedirectToAction("Login", "Admin");
            }

            ViewBag.TotalProductos = _context.Productos.Count();

            ViewBag.TotalPedidos = _context.Pedidos.Count();

            ViewBag.PedidosPendientes = _context.Pedidos
                .Count(p => p.Estado == "Pendiente");

            ViewBag.PedidosEntregados = _context.Pedidos
                .Count(p => p.Estado == "Entregado");

            ViewBag.VentasTotales = _context.Pedidos
                .Sum(p => (double?)p.Total) ?? 0;

            return View();
        }
    }
}