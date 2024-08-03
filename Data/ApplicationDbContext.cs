using CaffeBar.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace CaffeBar.Data;

public class CaffeBarDbContext : IdentityDbContext<ApplicationUser>
{
    public CaffeBarDbContext(DbContextOptions<CaffeBarDbContext> options)
        : base(options)
    {
    }
}
