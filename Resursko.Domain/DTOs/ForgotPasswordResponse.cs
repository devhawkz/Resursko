namespace Resursko.Domain.DTOs;

public record class ForgotPasswordResponse(bool isSuccessful, string? ErrorMessage = null);
