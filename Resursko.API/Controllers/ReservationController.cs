﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Resursko.API.Services.ReservationService;
using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize(Roles = "Admin, User")]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult<List<GetAllReservationResponse>>> GetAllReservations()
    {
        var result = await reservationService.GetAllReservations();
        if (result is not null && result.Count > 0)
            return Ok(result);

        return NotFound("There is no reservation in database");
    }
}
