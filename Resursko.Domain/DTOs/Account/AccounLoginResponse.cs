namespace Resursko.Domain.DTOs.Account;

public record class AccountLoginResponse(bool IsSuccessful, string Token = null!, string ErrorMessage = null!);

