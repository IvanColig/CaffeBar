using System.ComponentModel.DataAnnotations.Schema;

namespace CaffeBar.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string PriceHuman => Price.ToString("F2") + " €";
        public string? ImagePath { get; set; }
        [NotMapped]
        public IFormFile? Image { get; set; }
    }
}