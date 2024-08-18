using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Resursko.Client.Services.Account;

namespace Resursko.Client.Respositories.HttpRespository;

public class RefreshTokenService(IAccountService accountService, AuthenticationStateProvider authStateProvider, NavigationManager navManager)
{
    public async Task<string> TryRefreshToken()
    {
        var authState = await authStateProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (!user.Identity!.IsAuthenticated)
        {
            // Preusmeri na stranicu za prijavu
            navManager.NavigateTo("/login"); 
            return string.Empty;
        }

        var exp = user.FindFirst(c => c.Type.Equals("exp"))!.Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var timeUTC = DateTime.UtcNow;
        var diff = expTime - timeUTC;
        if (diff.TotalMinutes <= 2)
            return await accountService.RefreshToken();
        return string.Empty;
    }
}
