using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.EmailService;
using Resursko.Domain.DTOs;

namespace Resursko.API.Services.ForgotPasswotdService;

public class ForgotPasswordService(UserManager<User> userManager, IEmailSenderAsync emailSender, IUrlHelper urlHelper) : IForgotPasswordService
{
    public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPassword request)
    {
        var user = await userManager.FindByEmailAsync(request.Email!);
        if (user is null)
            return new ForgotPasswordResponse(false, $"User with this {request.Email} email address doesn't exist!");
        
        var token = await userManager.GeneratePasswordResetTokenAsync(user);

        var resetUrl = urlHelper.Link("ResetPassword", new { token, user.Email });
        var message = new Message(new List<string> { user.Email! }, "Reset password token", $"Follow the link bellow to reset your password.\n\n{resetUrl}");
        await emailSender.SendEmailAsync(message);

        return new ForgotPasswordResponse(true);
    }
}
