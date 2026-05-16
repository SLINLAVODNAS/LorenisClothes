using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using LorenisClothes.Data;
using LorenisClothes.Models;
using System.Linq;

namespace LorenisClothes.Controllers
{
    public class AdminController : Controller
    {
        private readonly AppDbContext _context;

        public AdminController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Login()
        {
            if (HttpContext.Session.GetString("Admin") != null)
            {
                return RedirectToAction("Index", "Pedidos");
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Login(string usuario, string password)
        {
            var admin = _context.Administradores
                .FirstOrDefault(u => u.Usuario == usuario && u.Password == password);

            if (admin != null)
            {
                HttpContext.Session.SetString("Admin", admin.Usuario);
                return RedirectToAction("Index", "Pedidos");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("Admin");
            return RedirectToAction("Index", "Home");
        }
    }
}