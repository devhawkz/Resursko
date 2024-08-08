namespace Resursko.Domain.DTOs;

public record class ResetPasswordResponse(bool isSuccesfull, string? ErrorMessage = null);
