namespace Resursko.Domain.DTOs.Account;

public record class AccountResponse(bool isSuccessful, string ErrorMessage = null!);
