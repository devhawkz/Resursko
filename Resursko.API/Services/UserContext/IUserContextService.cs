namespace Resursko.API.Services.UserContext;

public interface IUserContextService
{
    string? GetUserId();
    string? GetUserEmail();
    Task<User?> GetUserAsync();
}
