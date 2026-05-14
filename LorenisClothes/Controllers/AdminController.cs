using Microsoft.AspNetCore.Mvc;
using LorenisClothes.Data;
using Microsoft.AspNetCore.Http;
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

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Login(string usuario, string password)
        {
            if (_context == null)
            {
                return Content("Error de conexión con la base de datos");
            }

            var admin = _context.Administradores
                .FirstOrDefault(a =>
                    a.Usuario == usuario &&
                    a.Password == password);

            if (admin == null)
            {
                ViewBag.Error = "Usuario o contraseña incorrectos";
                return View();
            }

            HttpContext.Session.SetString("Admin", admin.Usuario);

            return RedirectToAction("Index", "Pedidos");
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();

            return RedirectToAction("Login");
        }
    }
}