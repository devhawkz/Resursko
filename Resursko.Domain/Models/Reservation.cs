namespace Resursko.Domain.Models;
public class Reservation
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public string? Status { get; set; }

    //relation with Resources
    public int ResourceId { get; set; }
    public Resource Resource { get; set; }

    // relation with User
    public string UserId { get; set; }
    public User User { get; set; }

    public void CheckStatus()
    {
        if (EndTime < DateTime.Now)
            Status = "inactive";

        else Status = "active";
    }
}
