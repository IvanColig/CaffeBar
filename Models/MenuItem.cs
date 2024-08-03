namespace CaffeBar.Models
{
    public class MenuItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Description { get; set; }
        public decimal Price { get; set; }
        public string PriceHuman {get => Price.ToString() + " €";}
        public string? Image { get; set; }
    }
}