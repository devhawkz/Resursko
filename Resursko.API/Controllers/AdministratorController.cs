using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.Account;

namespace Resursko.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AdministratorController(IAdminService adminService) : ControllerBase
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
    }
}
