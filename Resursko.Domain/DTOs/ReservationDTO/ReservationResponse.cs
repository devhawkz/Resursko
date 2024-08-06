namespace Resursko.Domain.DTOs.ReservationDTO;

public record class ReservationResponse(bool IsSuccessful, string? ErrorMessage = null);
