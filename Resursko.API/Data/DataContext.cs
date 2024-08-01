using Microsoft.EntityFrameworkCore;
using Resursko.Domain.Models;

namespace Resursko.API.Data;

public class DataContext(DbContextOptions<DataContext> options) : DbContext(options)
{
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<Resource> Resources { get; set; }
}
