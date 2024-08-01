using Microsoft.AspNetCore.Identity.EntityFrameworkCore;

namespace Resursko.API.Data;
public class DataContext(DbContextOptions<DataContext> options) : IdentityDbContext<User, Role, string>(options)
{
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Resource> Resources { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);
    }
}
