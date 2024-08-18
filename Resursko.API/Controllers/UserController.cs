using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.UsersServices;


namespace Resursko.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class UsersController(IUserServiceAPI userService) : ControllerBase
{
    [Authorize]
    [HttpPut("update-info")]
    public async Task<ActionResult<AccountLoginResponse>> UpdateUserInfo(UpdateUsersInfoRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data.");

        var result = await userService.UpdateUserInfo(request);

        if (result.IsSuccessful)
            return Ok(result);

        return BadRequest(result);
    }

    [Authorize]
    [HttpDelete("delete-account")]
    public async Task<ActionResult<AccountResponse>> DeleteAccount()
    {
        var result = await userService.DeleteAccount();

        if (result.isSuccessful)
            return Ok(result);

        return BadRequest(result);
    }

    [Authorize]
    [HttpGet("user-info")]
    public async Task<ActionResult<GetAllUsersResponse>> GetUserInfo()
    {
        var result = await userService.GetUserInfo();
        if (result is null)
            return NotFound("There is no user with this credentials.");

        return Ok(result);
    }
}
