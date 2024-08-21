using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Resursko.Client.AuthProviders;
using Resursko.Client.Services.Account;
using System.Diagnostics.CodeAnalysis;
using System.Security.Claims;

namespace Resursko.Client.Services.RefreshTokenServices;

public class RefreshTokenService
{
    private readonly AuthenticationStateProvider _authProvider;
    private readonly IAccountService _accountService;
    public RefreshTokenService(AuthenticationStateProvider authProvider, IAccountService accountService)
    {
        _accountService = accountService;
        _authProvider = authProvider;
    }

    public async Task<bool> TryRefreshToken()
    {
        var authState = await _authProvider.GetAuthenticationStateAsync();
        var user = authState.User;

        if (user is null || user.Identity is null || !user.Identity.IsAuthenticated) 
            return false;
            

        var exp = user.FindFirst(c => c.Type.Equals("exp"))!.Value;
        var expTime = DateTimeOffset.FromUnixTimeSeconds(Convert.ToInt64(exp));
        var timeUTC = DateTime.UtcNow;
        var diff = expTime - timeUTC;
        
        if (diff.TotalMinutes <= 2)
        {
            var result = await _accountService.RefreshToken();
            if(string.IsNullOrEmpty(result))
            {
                await _accountService.Logout();
                return false;
            }
        }
        return true;
    }
}
