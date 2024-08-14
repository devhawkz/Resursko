using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.Account;
using Resursko.API.Services.ForgotPasswotdService;
using Resursko.Domain.DTOs;

namespace Resursko.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountService accountService, IForgotPasswordService passwordService) : ControllerBase
{
    [HttpPost("registration")]
    public async Task<ActionResult<AccountRegistrationResponse>> Register(AccountRegistrationRequest request)
    {
        if(!ModelState.IsValid || request is null)
            return BadRequest("Invalid data entered");

        var result = await accountService.RegisterAsync(request);

        if(result.IsSuccessful)
            return Ok(result);

        return BadRequest(result.Errors);
    }

    [HttpPost]
    public async Task<ActionResult<AccountLoginResponse>> Login(AccountLoginRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest("Invalid data entered");

        var result = await accountService.LoginAsync(request);

        if (result.IsSuccessful)
            return Ok(result);

        return Unauthorized(result.ErrorMessage);
    }

    [HttpPost("forgot-password")]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword(ForgotPassword forgotPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data.");

        var result = await passwordService.ForgotPassword(forgotPassword);

        if (result.isSuccessful)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult<ResetPasswordResponse>> ResetPassword(ResetPassword resetPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data.");

        var result = await passwordService.ResetPassword(resetPassword);

        if (result.isSuccesfull)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpPost("refresh")]
    public async Task<ActionResult<TokenRefreshRequest>> RefreshToken(TokenRefreshRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid token");
        var result = await accountService.RefreshToken(request);

        return Ok(result);
    }
}
