using Microsoft.AspNetCore.Mvc;
using LorenisClothes.Data;
using LorenisClothes.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

namespace LorenisClothes.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string usuario, string password)
        {
            var admin = _context.Administradores
                .FirstOrDefault(u => u.Usuario == usuario && u.Password == password);

            if (admin != null)
            {
                HttpContext.Session.SetString("AdminSession", admin.Usuario);
                return RedirectToAction("Index", "Productos");
            }

            ViewBag.Error = "Credenciales incorrectas";
            return View();
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("AdminSession");
            return RedirectToAction("Index", "Login");
        }
    }
}