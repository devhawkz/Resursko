using Mapster;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Resursko.API.Services.EmailService;
using Resursko.API.Services.JwtHandler;
using Resursko.API.Services.UserContext;

namespace Resursko.API.Services.Account;
public class AccountService(UserManager<User> userManager, SignInManager<User> signInManager, JwtService jwtService, AccountServiceHelper serviceHelper, IEmailSenderAsync 
emailSender) : IAccountService
{
    private const string _roleName = "user"; 
    public async Task<AccountLoginResponse> LoginAsync(AccountLoginRequest request)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        if(user is null)
            return new AccountLoginResponse(false, ErrorMessage: "Invalid email or password!");

        var result = await signInManager.PasswordSignInAsync(user, request.Password!, false, false);
        if (!result.Succeeded)
            return new AccountLoginResponse(false, ErrorMessage: "Invalid email or password!");

        var roles = await userManager.GetRolesAsync(user);
        var refreshToken = jwtService.GenerateRefreshToken();
        user.RefreshToken = refreshToken;
        var jwtToken = await jwtService.CreateToken(user, roles, true);

        return new AccountLoginResponse(true, Token: jwtToken, RefreshToken: user.RefreshToken!);
    }

    public async Task<AccountRegistrationResponse> RegisterAsync(AccountRegistrationRequest request)
    {
        var newUser = serviceHelper.GetUser(request); 
        var result = await userManager.CreateAsync(newUser, request.Password!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return new AccountRegistrationResponse(false, errors);
        }

        var role = await serviceHelper.GetRoleAsync(_roleName);

        await userManager.AddToRoleAsync(newUser, role.Name!);

        var message = new Message(new string[] { request.Email! }, "User registration", "You have successfully register to our app!");

        await emailSender.SendEmailAsync(message);

        return new AccountRegistrationResponse(true);
    }


    public async Task<TokenRefreshRequest> RefreshToken(TokenRefreshRequest request)
    {
        var principal = jwtService.GetClaimsPrincipalFromExpiredToken(request.AccessToken);
        var user = await userManager.FindByNameAsync(principal.Identity!.Name!);
        if (user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.Now)
            return new TokenRefreshRequest(null!, null!);

        var roles = await userManager.GetRolesAsync(user);
        var token = await jwtService.CreateToken(user, roles, false);
        return new TokenRefreshRequest(AccessToken: token, RefreshToken: user.RefreshToken);
    }
}