using Mapster;
using MapsterMapper;
using Microsoft.AspNetCore.Identity;

namespace Resursko.API.Services.Account;

public class AccountService(UserManager<User> userManager) : IAccountService
{
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
