using Microsoft.AspNetCore.Identity;
using System.Security.Claims;

namespace Resursko.API.Services.UserContext;
public class UserContextService(IHttpContextAccessor contextAccessor, UserManager<User> userManager) : IUserContextService
{
    public async Task<User?> GetUserAsync()
    {
        var httpContextAccessor = contextAccessor.HttpContext?.User;
        if (httpContextAccessor is null)
            return null!;

        return await userManager.GetUserAsync(httpContextAccessor);
    }

    public string? GetUserId()
    {
        return contextAccessor.HttpContext?.User
            .FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
