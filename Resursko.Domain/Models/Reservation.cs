namespace Resursko.Domain.Models;
public class Reservation
{
    public int Id { get; set; }
    public DateTime StartTime { get; set; } = DateTime.Now;
    public DateTime EndTime { get; set; }
    public string? Status { get; set; }

    //relation with Resources
    public int ResourceId { get; set; }
    public Resource Resource { get; set; }

    public void CheckStatus()
    {
        if (EndTime < DateTime.Now)
            Status = "inactive";

        else Status = "active";
    }
}
