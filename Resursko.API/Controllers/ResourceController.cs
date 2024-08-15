using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.ResourceService;
using Resursko.Domain.DTOs.ResourceDTO;

namespace Resursko.API.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ResourceController(IServiceResoruce serviceResoruce) : ControllerBase
{
    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<ActionResult<ResourceResponse>> CreateResource(ResourceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data entered!");

        var result = await serviceResoruce.CreateResource(request);

        if(result.IsSuccessful)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }
    [Authorize(Roles = "Admin, User")]
    [HttpGet]
    public async Task<ActionResult<List<GetResourcesDTO>>> GetAllResources()
    {
        var result = await serviceResoruce.GetAllResources();

        if (result is not null && result.Count > 0)
            return Ok(result);

        return NotFound("There is no resource in database");
    }

    [Authorize(Roles = "Admin")]
    [HttpPut("{id}")]
    public async Task<ActionResult<ResourceResponse>> UpdateResource(ResourceRequest request, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data entered!");

        var result = await serviceResoruce.UpdateResource(request, id);
        
        if(result.IsSuccessful)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }

    [Authorize(Roles = "Admin")]
    [HttpDelete("{id}")]
    public async Task<ActionResult<ResourceResponse>> DeleteResource(int id)
    {
        var result = await serviceResoruce.DeleteResource(id);
        
        if (result.IsSuccessful)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }
}
