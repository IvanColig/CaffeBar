namespace CaffeBar.Models
{
    public class Reservation
    {
        public int Id { get; set; }
        public int TableId { get; set; }
        public required string IdentityUserId { get; set; }
	    public required ApplicationUser IdentityUser { get; set; }
    }
}