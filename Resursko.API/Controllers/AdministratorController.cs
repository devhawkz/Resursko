using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.Account;
using Resursko.API.Services.UsersServices;

namespace Resursko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController(IAdminService adminService, IUserService userService) : ControllerBase
    {
        [HttpPost]
        public async Task<ActionResult<AccountRegistrationResponse>> RegisterAdministrator(AccountRegistrationRequest request)
        {
            if (!ModelState.IsValid)
                return BadRequest("Invalid data entered");

            var result = await adminService.RegisterAdminAsync(request);

            if (result.IsSuccessful)
                return Ok(result);

            return BadRequest(result.Errors);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<List<GetAllUsersResponse>>> GetAllUsers()
        {
            var result = await userService.GetAllUsers();
            if (result is not null && result!.Count > 0)
                return Ok(result);

            return NotFound("There is no users registered in your app.");
        }
    }
}
