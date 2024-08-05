﻿using Microsoft.AspNetCore.Authorization;
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
    public async Task<ActionResult<ResourceResponse>> CreateResource(CreateResourceRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest("Invalid data entered!");

        var result = await serviceResoruce.CreateResource(request);

        if(result.IsSuccessful)
            return Ok(result);

        return BadRequest(result.ErrorMessage);
    }
}
