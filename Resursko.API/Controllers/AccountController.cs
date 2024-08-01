using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.Account;

namespace Resursko.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AccountController(IAccountService accountService) : ControllerBase
{
    [HttpPost("register")]
    public async Task<ActionResult<AccountRegistrationResponse>> Register(AccountRegistrationRequest request)
    {
        if(!ModelState.IsValid)
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
}
