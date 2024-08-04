namespace CaffeBar.Models
{
    public class Order
    {
        public int Id { get; set; }
        public required string IdentityUserId { get; set; }
        public required ApplicationUser IdentityUser { get; set; }
        public int TableId { get; set; }
        public ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
    }
}
