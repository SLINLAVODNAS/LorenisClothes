using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using LorenisClothes.Data;
using System.Linq;
using System.Threading.Tasks;

namespace LorenisClothes.Controllers
{
    public class TiendaController : Controller
    {
        private readonly AppDbContext _context;

        public TiendaController(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            
            var productos = await _context.Productos
                .Where(p => p.Stock > 0)
                .ToListAsync();

            return View(productos);
        }
    }
}