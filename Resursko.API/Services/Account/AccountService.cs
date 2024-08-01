using Microsoft.AspNetCore.Identity;

namespace Resursko.API.Services.Account;

public class AccountService(UserManager<User> userManager) : IAccountService
{
    public async Task<AccountRegistrationResponse> RegisterAsync(AccountRegistrationRequest request)
    {
        var newUser = new User
        {
            FirstName = request.FirstName!,
            LastName = request.LastName!,
            UserName = request.Username,
            Email = request.Email
        };

        var result = await userManager.CreateAsync(newUser, request.Password!);

        if (!result.Succeeded)
        {
            var errors = result.Errors.Select(e => e.Description);
            return new AccountRegistrationResponse(false, errors);
        }

        return new AccountRegistrationResponse(true);
    }
}
