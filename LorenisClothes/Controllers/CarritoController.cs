using Microsoft.AspNetCore.Mvc;
using LorenisClothes.Models;
using LorenisClothes.Data;
using System.Collections.Generic;
using System.Linq;

namespace LorenisClothes.Controllers
{
    public class CarritoController : Controller
    {
        private static List<CarritoItem> carrito = new List<CarritoItem>();

        private readonly AppDbContext _context;

        public CarritoController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View(carrito);
        }

        public IActionResult Agregar(int id, string nombre, double precio, string imagenUrl)
        {
            var producto = _context.Productos.FirstOrDefault(x => x.Id == id);

            if (producto == null)
            {
                return RedirectToAction("Index", "Productos");
            }

            if (producto.Stock <= 0)
            {
                return RedirectToAction("Index", "Productos");
            }

            var item = carrito.FirstOrDefault(x => x.ProductoId == id);

            if (item != null)
            {
                if (item.Cantidad < producto.Stock)
                {
                    item.Cantidad++;
                }
            }
            else
            {
                carrito.Add(new CarritoItem
                {
                    ProductoId = id,
                    Nombre = nombre,
                    Precio = precio,
                    ImagenUrl = imagenUrl,
                    Cantidad = 1
                });
            }

            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            var item = carrito.FirstOrDefault(x => x.ProductoId == id);

            if (item != null)
            {
                carrito.Remove(item);
            }

            return RedirectToAction("Index");
        }

        public IActionResult Vaciar()
        {
            carrito.Clear();

            return RedirectToAction("Index");
        }

        public IActionResult Checkout()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Checkout(Pedido pedido)
        {
            if (carrito.Count == 0)
            {
                return RedirectToAction("Index");
            }

            pedido.Detalles = new List<DetallePedido>();

            double total = 0;

            foreach (var item in carrito)
            {
                var producto = _context.Productos.FirstOrDefault(x => x.Id == item.ProductoId);

                if (producto == null || producto.Stock < item.Cantidad)
                {
                    return Content("Stock insuficiente para: " + item.Nombre);
                }

                producto.Stock -= item.Cantidad;

                total += item.Precio * item.Cantidad;

                pedido.Detalles.Add(new DetallePedido
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad
                });
            }

            pedido.Total = total;

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            carrito.Clear();

            return RedirectToAction("Index", "Productos");
        }

        public IActionResult WhatsApp()
        {
            if (carrito.Count == 0)
            {
                return RedirectToAction("Index");
            }

            string numero = "50245556147";

            string mensaje = "Hola, quiero hacer este pedido:%0A";

            double total = 0;

            foreach (var item in carrito)
            {
                mensaje += $"- {item.Nombre} x{item.Cantidad}%0A";
                total += item.Precio * item.Cantidad;
            }

            mensaje += $"%0ATotal: Q{total}";

            string url = $"https://wa.me/{numero}?text={mensaje}";

            return Redirect(url);
        }
    }
}