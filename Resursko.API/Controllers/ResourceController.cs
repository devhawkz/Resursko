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

    [HttpGet]
    public async Task<ActionResult<List<Resource>>> GetAllResources()
    {
        var result = await serviceResoruce.GetAllResources();

        if (result is not null)
            return Ok(result);

        return BadRequest("The resource table is empty");
    }

    
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
}
