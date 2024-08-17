namespace Resursko.Domain.DTOs.ReservationDTO;
public class GetAllReservationResponse
{
    public int Id {  get; set; }
    public string?  Username { get; set; }
    public string? Email { get; set; }
    public string? ResourceName { get; set; }
    public DateTime? StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public string? Status { get; set; }
}
