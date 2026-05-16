using Microsoft.AspNetCore.Mvc;
using LorenisClothes.Models;
using LorenisClothes.Data;
using LorenisClothes.Extensions;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System;
using System.Net;

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
            var carrito = ObtenerCarrito();
            ViewBag.Total = carrito.Sum(x => x.Precio * x.Cantidad);
            return View(carrito);
        }

        public IActionResult Agregar(int id)
        {
            var producto = _context.Productos.Find(id);
            if (producto == null) return RedirectToAction("Index", "Home");

            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(x => x.ProductoId == id);

            if (item != null)
            {
                if (item.Cantidad < producto.Stock)
                {
                    item.Cantidad++;
                }
                else
                {
                    TempData["Error"] = $"No hay más unidades disponibles de {producto.Nombre}";
                }
            }
            else
            {
                if (producto.Stock > 0)
                {
                    carrito.Add(new CarritoItem
                    {
                        ProductoId = id,
                        Nombre = producto.Nombre,
                        Precio = producto.Precio,
                        ImagenUrl = producto.ImagenUrl,
                        Cantidad = 1
                    });
                }
                else
                {
                    TempData["Error"] = "El producto se encuentra agotado.";
                }
            }

            GuardarCarrito(carrito);
            return RedirectToAction("Index");
        }

        // Nueva acción para corregir el error 404 al restar
        public IActionResult Restar(int id)
        {
            var carrito = ObtenerCarrito();
            var item = carrito.FirstOrDefault(x => x.ProductoId == id);

            if (item != null)
            {
                if (item.Cantidad > 1)
                {
                    item.Cantidad--;
                }
                else
                {
                    carrito.Remove(item);
                }
                GuardarCarrito(carrito);
            }

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

        public IActionResult Checkout()
        {
            var carrito = ObtenerCarrito();
            if (carrito.Count == 0) return RedirectToAction("Index");

            ViewBag.Total = carrito.Sum(x => x.Precio * x.Cantidad);
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Checkout(Pedido pedido)
        {
            var carrito = ObtenerCarrito();
            if (carrito.Count == 0) return RedirectToAction("Index");

            if (!ModelState.IsValid)
            {
                ViewBag.Total = carrito.Sum(x => x.Precio * x.Cantidad);
                return View(pedido);
            }

            pedido.FechaPedido = DateTime.Now;
            pedido.Estado = "Pendiente";
            double totalCalculado = 0;

            pedido.Detalles = new List<DetallePedido>();

            foreach (var item in carrito)
            {
                var producto = _context.Productos.Find(item.ProductoId);
                if (producto == null || producto.Stock < item.Cantidad)
                {
                    TempData["Error"] = $"Stock insuficiente para: {item.Nombre}";
                    return RedirectToAction("Index");
                }

                producto.Stock -= item.Cantidad;
                totalCalculado += item.Precio * item.Cantidad;

                pedido.Detalles.Add(new DetallePedido
                {
                    ProductoId = item.ProductoId,
                    Cantidad = item.Cantidad,
                    PrecioUnitario = item.Precio
                });
            }

            pedido.Total = totalCalculado;

            _context.Pedidos.Add(pedido);
            _context.SaveChanges();

            string numero = "50245556147";
            string textoMensaje = $"*NUEVO PEDIDO #00{pedido.Id}*\n" +
                                  $"*Cliente:* {pedido.NombreCliente}\n" +
                                  $"*Tel:* {pedido.Telefono}\n" +
                                  $"*Dirección:* {pedido.Direccion}\n" +
                                  "--------------------------\n";

            foreach (var item in carrito)
            {
                textoMensaje += $"• {item.Nombre} (x{item.Cantidad}) - Q{item.Precio * item.Cantidad:N2}\n";
            }

            textoMensaje += "--------------------------\n" +
                            $"*TOTAL A PAGAR: Q{totalCalculado:N2}*\n\n" +
                            "_Entrega local en cabecera de Jalapa._";

            string mensajeSafe = WebUtility.UrlEncode(textoMensaje);

            HttpContext.Session.Remove("Carrito");

            return Redirect($"https://wa.me/{numero}?text={mensajeSafe}");
        }
    }
}