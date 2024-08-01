using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using Resursko.API.Services.JwtHandler;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Resursko.API.Services.Account;

public class AccountService(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService) : IAccountService
{
    public async Task<AccountLoginResponse> LoginAsync(AccountLoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        if (user is null || !await userManager.CheckPasswordAsync(user, request.Password!))
            return new AccountLoginResponse(false, ErrorMessage: "Invalid email or password!");

        var result = await signInManager.PasswordSignInAsync(user, request.Password!, false, false);
        if (!result.Succeeded)
            return new AccountLoginResponse(false, ErrorMessage: "An error occurred while trying to sign you in. Please try again!");

        var jwtToken = jwtService.CreateToken(user);

        return new AccountLoginResponse(true, Token:jwtToken);
    }

    public async Task<AccountRegistrationResponse> RegisterAsync(AccountRegistrationRequest request)
    {
        var newUser =  request.Adapt<User>();
        newUser.UserName = request.Username;
        var result = await userManager.CreateAsync(newUser, request.Password!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return new AccountRegistrationResponse(false, errors);
        }

        return new AccountRegistrationResponse(true);
    }
}
