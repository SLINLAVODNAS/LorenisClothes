using LorenisClothes.Data;
using LorenisClothes.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace LorenisClothes.Controllers
{
    public class ProductosController : Controller
    {
        private readonly AppDbContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductosController(AppDbContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
        }

        public async Task<IActionResult> Index(string buscar, string categoria)
        {
            var categorias = await _context.Productos
                .Where(p => !string.IsNullOrEmpty(p.Categoria))
                .Select(p => p.Categoria)
                .Distinct()
                .ToListAsync();

            ViewBag.Categorias = categorias;
            var productos = _context.Productos.AsQueryable();

            if (!string.IsNullOrEmpty(buscar))
                productos = productos.Where(s => s.Nombre.Contains(buscar) || s.Descripcion.Contains(buscar));

            if (!string.IsNullOrEmpty(categoria))
                productos = productos.Where(x => x.Categoria == categoria);

            return View(await productos.OrderByDescending(p => p.Id).ToListAsync());
        }

        public IActionResult Create()
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true")
            {
                return RedirectToAction("Login", "Admin"); 
            }
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Producto producto)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") return Unauthorized();

            ModelState.Remove("ImagenUrl");
            ModelState.Remove("ImagenArchivo");

            if (producto.ImagenArchivo != null)
            {
                producto.ImagenUrl = await GuardarImagen(producto.ImagenArchivo);
            }

            if (ModelState.IsValid)
            {
                _context.Add(producto);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        public async Task<IActionResult> Edit(int? id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") return RedirectToAction("Login", "Admin");
            if (id == null) return NotFound();

            var producto = await _context.Productos.FindAsync(id);
            return producto == null ? NotFound() : View(producto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Producto producto)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") return Unauthorized();

            ModelState.Remove("ImagenUrl");
            ModelState.Remove("ImagenArchivo");

            if (id != producto.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    if (producto.ImagenArchivo != null)
                    {
                        var ant = await _context.Productos.AsNoTracking().FirstOrDefaultAsync(p => p.Id == id);
                        if (ant != null) EliminarArchivoImagen(ant.ImagenUrl);
                        producto.ImagenUrl = await GuardarImagen(producto.ImagenArchivo);
                    }
                    else
                    {
                        _context.Entry(producto).Property(x => x.ImagenUrl).IsModified = false;
                    }

                    _context.Update(producto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductoExists(producto.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(producto);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (HttpContext.Session.GetString("IsAdmin") != "true") return Unauthorized();

            var producto = await _context.Productos.FindAsync(id);
            if (producto != null)
            {
                EliminarArchivoImagen(producto.ImagenUrl);
                _context.Productos.Remove(producto);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

        private async Task<string> GuardarImagen(Microsoft.AspNetCore.Http.IFormFile archivo)
        {
            string carpeta = Path.Combine(_webHostEnvironment.WebRootPath, "imagenes");
            if (!Directory.Exists(carpeta)) Directory.CreateDirectory(carpeta);
            string nombre = Guid.NewGuid().ToString() + Path.GetExtension(archivo.FileName);
            string ruta = Path.Combine(carpeta, nombre);
            using (var stream = new FileStream(ruta, FileMode.Create)) { await archivo.CopyToAsync(stream); }
            return "/imagenes/" + nombre;
        }

        private void EliminarArchivoImagen(string url)
        {
            if (string.IsNullOrEmpty(url)) return;
            string ruta = Path.Combine(_webHostEnvironment.WebRootPath, url.TrimStart('/'));
            if (System.IO.File.Exists(ruta)) System.IO.File.Delete(ruta);
        }

        private bool ProductoExists(int id) => _context.Productos.Any(e => e.Id == id);
    }
}