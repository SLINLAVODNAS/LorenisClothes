using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LorenisClothes.Models
{
    public class Producto
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "El nombre es obligatorio")]
        public string Nombre { get; set; } = string.Empty;

        [Required(ErrorMessage = "La descripción es obligatoria")]
        public string Descripcion { get; set; } = string.Empty;

        [Required(ErrorMessage = "El precio es obligatorio")]
        [Range(0.01, double.MaxValue, ErrorMessage = "El precio debe ser mayor a 0")]
        public double Precio { get; set; }

        [Required(ErrorMessage = "El color es obligatorio")]
        public string Color { get; set; } = string.Empty;

        [Required(ErrorMessage = "La talla es obligatoria")]
        public string Talla { get; set; } = string.Empty;

        public string? Categoria { get; set; }

        [Required(ErrorMessage = "El stock es obligatorio")]
        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo")]
        public int Stock { get; set; }

        public string? ImagenUrl { get; set; }

        [NotMapped]
        public IFormFile? ImagenArchivo { get; set; }
    }
}