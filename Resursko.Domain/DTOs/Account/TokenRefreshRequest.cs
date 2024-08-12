namespace Resursko.Domain.DTOs.Account;

public record class TokenRefreshRequest(string AccessToken, string RefreshToken);

