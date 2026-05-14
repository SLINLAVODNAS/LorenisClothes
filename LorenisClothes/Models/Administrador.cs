using System.ComponentModel.DataAnnotations;

namespace LorenisClothes.Models
{
    public class Administrador
    {
        public int Id { get; set; }

        [Required]
        public string Usuario { get; set; } = "";

        [Required]
        public string Password { get; set; } = "";
    }
}