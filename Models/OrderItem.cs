namespace CaffeBar.Models
{
    public class OrderItem
    {
        public int OrderId { get; set; }
        public int MenuItemId { get; set; }
        public int Quantity { get; set; }
        public Order Order { get; set; } = null!;
        public MenuItem MenuItem { get; set; } = null!;
    }
}