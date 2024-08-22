using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Resursko.API.Services.ReservationService;
using Resursko.Domain.DTOs.ReservationDTO;

namespace Resursko.API.Controllers;

[Route("api/[controller]")]
[ApiController]
[Authorize]
public class ReservationController(IReservationService reservationService) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<ReservationResponse>> CreateNewReservation(ReservationRequest request)
    {
        if(!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await reservationService.CreateNewReservation(request);

        if(result.IsSuccessful)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet]
    public async Task<ActionResult<List<GetAllReservationResponse>>> GetAllReservations()
    {
        var result = await reservationService.GetAllReservations();
        if (result is not null && result.Count > 0)
            return Ok(result);

        return NotFound("There is no reservation in database");
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<ReservationResponse>> UpdateReservation(ReservationRequest request, int id)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);
        
        var result = await reservationService.UpdateReservation(request, id);

        if(result.IsSuccessful)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult<ReservationResponse>> DeleteReservation(int id)
    {
        var result = await reservationService.DeleteReservation(id);

        if (result.IsSuccessful)
            return Ok(result);

        return BadRequest(result);
    }

    [HttpGet("reservations-resource/{id}")]
    public async Task<ActionResult<List<GetAllReservationResponse>>> GetReservationsByResourceId(int id)
    {
        if (id <= -1)
            return BadRequest();

        var result = await reservationService.GetReservationsByResource(id);
        if (result is not null && result.Count > 0)
            return Ok(result);

        return NotFound("There is no reservation made for this resource in database.");
    }
}
