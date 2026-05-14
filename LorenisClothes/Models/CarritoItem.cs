namespace LorenisClothes.Models
{
    public class CarritoItem
    {
        public int ProductoId { get; set; }
        public string Nombre { get; set; } = "";
        public double Precio { get; set; }
        public string? ImagenUrl { get; set; }
        public int Cantidad { get; set; }
    }
}