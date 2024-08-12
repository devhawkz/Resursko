namespace Resursko.Domain.DTOs.Account;

public record class AccountLoginResponse(bool IsSuccessful, string Token = null!, string RefreshToken = null!, string ErrorMessage = null!);

