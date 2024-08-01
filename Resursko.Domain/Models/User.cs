using Microsoft.AspNetCore.Identity;

namespace Resursko.Domain.Models;

public class User : IdentityUser
{
    public string? FirstName {  get; set; }
    public string? LastName { get; set; }
    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
}
