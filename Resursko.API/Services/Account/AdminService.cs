using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Resursko.API.Services.EmailService;

namespace Resursko.API.Services.Account;

public class AdminService(UserManager<User> userManager, AccountServiceHelper serviceHelper, IEmailSenderAsync emailSender) : IAdminService
{
    private const string _roleName = "admin";
    public async Task<AccountRegistrationResponse> RegisterAdminAsync(AccountRegistrationRequest request)
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
}
