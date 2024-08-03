namespace CaffeBar.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string? IdentityUserId { get; set; }
        public ApplicationUser? IdentityUser { get; set; }
        public int TableId { get; set; }
        public ICollection<OrderItem> OrderItems { get; } = new List<OrderItem>();
    }
}
