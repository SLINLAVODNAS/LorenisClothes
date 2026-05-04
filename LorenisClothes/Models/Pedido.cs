using System.ComponentModel.DataAnnotations;

namespace LorenisClothes.Models
{
    public class Pedido
    {
        public int Id { get; set; }

        [Required]
        public string NombreCliente { get; set; }

        [Required]
        public string Telefono { get; set; }

        [Required]
        public string Direccion { get; set; }

        public DateTime FechaPedido { get; set; } = DateTime.Now;

        public string Estado { get; set; } = "Pendiente";

        public double Total { get; set; }
    }
}