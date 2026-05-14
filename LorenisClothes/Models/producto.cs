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

        [Required]
        public string Talla { get; set; } = "";

        public int Stock { get; set; }

        public string? ImagenUrl { get; set; }

        [NotMapped]
        public IFormFile? ImagenArchivo { get; set; }
    }
}