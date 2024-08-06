using System.ComponentModel.DataAnnotations;

namespace Resursko.Domain.DTOs.ReservationDTO;

public class ReservationRequest
{
    [Required]
    public string? UserId { get; set; }
    [Required]
    public string? ResourceId { get; set; }
    [Required, DataType(DataType.DateTime)]
    public DateTime StartTime { get; set; }
    [Required, DataType(DataType.DateTime)]
    public DateTime EndTime { get; set; }

}
