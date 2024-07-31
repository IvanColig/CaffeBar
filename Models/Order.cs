namespace CaffeBar.Models
{
    public class Order
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int TableId { get; set; }
        public ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
    }
}