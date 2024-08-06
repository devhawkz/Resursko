namespace Resursko.API.Services.UserContext;

public interface IUserContextService
{
    string? GetUserId();
    Task<User?> GetUserAsync();
}
