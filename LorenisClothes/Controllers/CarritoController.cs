using Microsoft.AspNetCore.Mvc;
using LorenisClothes.Models;
using LorenisClothes.Data;
using LorenisClothes.Extensions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;

namespace LorenisClothes.Controllers
{
    public class CarritoController : Controller
    {
        private readonly AppDbContext _context;

        public CarritoController(AppDbContext context)
        {
            _context = context;
        }

        private List<CarritoItem> ObtenerCarrito()
        {
            var carrito = HttpContext.Session.GetObjectFromJson<List<CarritoItem>>("Carrito");
            return carrito ?? new List<CarritoItem>();
        }

        private void GuardarCarrito(List<CarritoItem> carrito)
        {
            HttpContext.Session.SetObjectAsJson("Carrito", carrito);
        }

        public IActionResult Index()
        {
            return View(ObtenerCarrito());
        }

        public IActionResult Agregar(int id, string nombre, double precio, string imagenUrl)
        {
            var producto = _context.Productos.FirstOrDefault(x => x.Id == id);
            if (producto == null || producto.Stock <= 0) return RedirectToAction("Index", "Productos");

            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(x => x.ProductoId == id);

            if (item != null)
            {
                if (item.Cantidad < producto.Stock) item.Cantidad++;
            }
            else
            {
                carrito.Add(new CarritoItem { ProductoId = id, Nombre = nombre, Precio = precio, ImagenUrl = imagenUrl, Cantidad = 1 });
            }

            GuardarCarrito(carrito);
            return RedirectToAction("Index");
        }

        public IActionResult Eliminar(int id)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(x => x.ProductoId == id);
            if (item != null)
            {
                carrito.Remove(item);
                GuardarCarrito(carrito);
            }
            return RedirectToAction("Index");
        }

        public IActionResult Vaciar()
        {
            HttpContext.Session.Remove("Carrito");
            return RedirectToAction("Index");
        }

        public IActionResult Checkout() => View();

        [HttpPost]
        public IActionResult Checkout(Pedido pedido)
        {
            var carrito = ObtenerCarrito();
            if (carrito.Count == 0) return RedirectToAction("Index");

            pedido.Detalles = new List<DetallePedido>();
            pedido.FechaPedido = DateTime.Now;
            double total = 0;

            foreach (var item in carrito)
            {
                var producto = _context.Productos.Find(item.ProductoId);
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

            string numero = "50245556147";
            string encabezado = "*PEDIDO %23" + pedido.Id + "*%0A*Cliente:* " + pedido.NombreCliente + "%0A*Tel:* " + pedido.Telefono + "%0A--------------------------%0A";
            string cuerpo = "";

            foreach (var item in carrito)
            {
                cuerpo += "- " + item.Nombre + " (x" + item.Cantidad + ") - Q" + (item.Precio * item.Cantidad) + "%0A";
            }

            string pie = "--------------------------%0A*TOTAL: Q" + total + "*";
            string mensajeCompleto = encabezado + cuerpo + pie;

            HttpContext.Session.Remove("Carrito");

            return Redirect("https://wa.me/" + numero + "?text=" + mensajeCompleto);
        }
    }
}