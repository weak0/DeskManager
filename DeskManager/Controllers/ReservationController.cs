using System.Security.Claims;
using DeskManager.Entities;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskManager.Controllers;

[ApiController]
[Route("desks/{deskId:int}/reservation")]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<ActionResult<Desk>> MakeReservation([FromRoute] int deskId, [FromBody] ReservationDto dto)
    {
        var desk = await _reservationService.MakeReservation(deskId, dto);
        return Ok(desk);
    }

    [HttpDelete("{reservationId:int}")]
    public async Task<ActionResult<Desk>> CancelReservation([FromRoute] int deskId, [FromRoute] int reservationId)
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value); 
        var desk = await _reservationService.CancelReservation(deskId, reservationId);
        return NoContent();
    }
    
}