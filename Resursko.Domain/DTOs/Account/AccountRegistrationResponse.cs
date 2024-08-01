namespace Resursko.Domain.DTOs.Account;
public record class AccountRegistrationResponse(bool IsSuccessful, IEnumerable<string?> Errors = null!);
