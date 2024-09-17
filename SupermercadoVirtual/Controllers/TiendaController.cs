using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using SupermercadoVirtual.Models;
using Microsoft.AspNetCore.Http;
using System.Linq;

[Route("tienda")] // Definir un prefijo para todas las rutas de TiendaController
public class TiendaController : Controller
{
    // Productos de ejemplo
    private static List<Producto> productos = new List<Producto>
    {
        new Producto { Id = 1, Nombre = "Atún VanCamps", Precio = 12, Imagen = "atun.png" },
        new Producto { Id = 2, Nombre = "Queso menonita", Precio = 45, Imagen = "queso.png" }
    };

    [HttpGet]
    [Route("")]
    [Route("index")]
    public IActionResult Index()
    {
        return View(productos);
    }

    [HttpPost]
    [Route("agregar-al-carrito")]
    public IActionResult AgregarAlCarrito(int id)
    {
        var producto = productos.FirstOrDefault(p => p.Id == id);
        if (producto == null) return NotFound();

        List<Elemento> carrito = ObtenerCarrito();
        var elemento = carrito.FirstOrDefault(e => e.Producto.Id == id);

        if (elemento == null)
        {
            carrito.Add(new Elemento { Producto = producto, Cantidad = 1 });
        }
        else
        {
            elemento.Cantidad++;
        }

        GuardarCarrito(carrito);
        return RedirectToAction("Index");
    }

    [HttpGet]
    [Route("VerCarrito")]
    public IActionResult VerCarrito()
    {
        var carrito = ObtenerCarrito();
        double total = carrito.Sum(e => e.Producto.Precio * e.Cantidad);
        ViewBag.Total = total;
        return View(carrito);
    }

    [HttpGet]
    [Route("FinalizarCompra")]
    public IActionResult FinalizarCompra()
    {
        var carrito = ObtenerCarrito();
        double total = carrito.Sum(e => e.Producto.Precio * e.Cantidad);
        HttpContext.Session.Clear();
        ViewBag.Total = total;
        return View();
    }

    private List<Elemento> ObtenerCarrito()
    {
        var carrito = HttpContext.Session.GetObjectFromJson<List<Elemento>>("Carrito");
        return carrito ?? new List<Elemento>();
    }

    private void GuardarCarrito(List<Elemento> carrito)
    {
        HttpContext.Session.SetObjectAsJson("Carrito", carrito);
    }

    [HttpPost]
    [Route("eliminar-del-carrito")]
    public IActionResult EliminarDelCarrito(int id)
    {
        List<Elemento> carrito = ObtenerCarrito();
        var elemento = carrito.FirstOrDefault(e => e.Producto.Id == id);

        if (elemento != null)
        {
            if (elemento.Cantidad > 1)
            {
                elemento.Cantidad--;
            }
            else
            {
                carrito.Remove(elemento);
            }

            GuardarCarrito(carrito);
        }

        return RedirectToAction("VerCarrito");
    }
}