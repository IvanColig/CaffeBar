namespace CaffeBar.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public string? IdentityUserId { get; set; }
	    public ApplicationUser? IdentityUser { get; set; }
    }
}