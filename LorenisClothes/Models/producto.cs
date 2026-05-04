using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LorenisClothes.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required]
        public string Nombre { get; set; } = "";

        [Required]
        public string Descripcion { get; set; } = "";

        [Required]
        public double Precio { get; set; }

        [Required]
        public string Color { get; set; } = "";

        // Talla principal del producto
        // Ejemplo: 1, 3, 5, 7

        [Required]
        public string Talla { get; set; } = "";

        // Aquí pondremos tallas y stock manualmente
        // Ejemplo:
        // 1 = 3 unidades
        // 3 = 5 unidades
        // 5 = 2 unidades

        [Required]
        public string TallasTexto { get; set; } = "";

        public string? ImagenUrl { get; set; }

        [NotMapped]
        public IFormFile? ImagenArchivo { get; set; }
    }
}