namespace Resursko.Domain.Models;

public class User
{
    public string FirstName {  get; set; }
    public string LastName { get; set; }

    public List<Reservation> Reservations { get; set; } = new List<Reservation>();
}
