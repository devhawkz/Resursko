using Microsoft.AspNetCore.Identity;

namespace Resursko.API.Services.Account
{
    public class AdminService(UserManager<User> userManager, AccountServiceHelper serviceHelper) : IAdminService
    {
        public async Task<AccountRegistrationResponse> RegisterAdminAsync(AccountRegistrationRequest request)
        {
            var newUser = serviceHelper.GetUser(request);
            var result = await userManager.CreateAsync(newUser, request.Password!);

            if (!result.Succeeded)
            {
                var errors = result.Errors.Select(e => e.Description);
                return new AccountRegistrationResponse(false, errors);
            }

            var role = await serviceHelper.GetRoleAsync("admin");

            await userManager.AddToRoleAsync(newUser, role.Name!);

            return new AccountRegistrationResponse(true);
        }
    }
}
