using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.ForgotPasswotdService;
using Resursko.Domain.DTOs;

namespace Resursko.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PasswordController(IForgotPasswordService forgotPasswordService) : ControllerBase
{
    [HttpPost("forgot-password")]
    public async Task<ActionResult<ForgotPasswordResponse>> ForgotPassword(ForgotPassword forgotPassword)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data.");
        
        var result = await forgotPasswordService.ForgotPassword(forgotPassword);

        if (result is not null)
            return Ok(result);

        return BadRequest(result);
    }
}
