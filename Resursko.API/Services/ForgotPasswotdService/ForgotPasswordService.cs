using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Routing;
using Resursko.API.Services.EmailService;
using Resursko.Domain.DTOs;
using System;

namespace Resursko.API.Services.ForgotPasswotdService;

public class ForgotPasswordService : IForgotPasswordService
{
    private UserManager<User> _userManager;
    private IEmailSenderAsync _emailSender;
    private IUrlHelper _urlHelper;

    public ForgotPasswordService(UserManager<User> userManager, IEmailSenderAsync emailSender, IUrlHelperFactory urlHelperFactory, IHttpContextAccessor httpContextAccessor)
    {
        _userManager = userManager;
        _emailSender = emailSender;

        // in order to use IUrlHelper object
        //1. gets current http context 
        //2. gets route info
        //3. represents meta data about action that is performing.
        //4. creating an instance of IUserHelper
        var actionContext = new ActionContext
            (
                httpContextAccessor.HttpContext!,
                httpContextAccessor.HttpContext!.GetRouteData(),
                new Microsoft.AspNetCore.Mvc.Abstractions.ActionDescriptor()
            );

        _urlHelper = urlHelperFactory.GetUrlHelper(actionContext);
    }
    public async Task<ForgotPasswordResponse> ForgotPassword(ForgotPassword request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user is null)
            return new ForgotPasswordResponse(false, $"User with this {request.Email} email address doesn't exist!");
        
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);

        var resetUrl = _urlHelper.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, "https");

        var message = new Message(new List<string> { user.Email! }, "Reset password token", $"Follow the link bellow to reset your password.\n\n{resetUrl}");
        await _emailSender.SendEmailAsync(message);

        return new ForgotPasswordResponse(true);
    }

    public async Task<ResetPasswordResponse> ResetPassword(ResetPassword request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email!);
        if (user is null)
            return new ResetPasswordResponse(false, $"User with {request.Email} email address doesn't exist!");

        var resetPassword = await _userManager.ResetPasswordAsync(user, request.ResetToken!, request.Password!);
        if (!resetPassword.Succeeded)
            return new ResetPasswordResponse(false, "Error occurred. Please try again later");

        var message = new Message(new List<string> { user.Email! }, "Reset password successfully", $"You have reset password successfully!");
        await _emailSender.SendEmailAsync(message);

        return new ResetPasswordResponse(true);

    }
}
