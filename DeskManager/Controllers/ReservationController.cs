using System.Security.Claims;
using DeskManager.Entities;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskManager.Controllers;

[ApiController]
[Route("/reservations/{deskId:int}")]
[Authorize]
public class ReservationController : ControllerBase
{
    private readonly IReservationService _reservationService;

    public ReservationController(IReservationService reservationService)
    {
        _reservationService = reservationService;
    }

    [HttpPost]
    public async Task<ActionResult<ReservationDto>> MakeReservation([FromRoute] int deskId, [FromBody] CreateReservationDto dto)
    {
        var desk = await _reservationService.MakeReservation(deskId, dto);
        return Ok(desk);
    }

    [HttpPost("oneDay")]
    public async Task<ActionResult<ReservationDto>> OneDayReservation([FromRoute] int deskId, [FromBody] CreateShortReservationDto dto)
    {
        var desk = await _reservationService.ShortReservation(deskId, dto);
        return Ok(desk);
    }

    [HttpDelete("/reservations/{reservationId:int}")]
    public async Task<ActionResult> CancelReservation([FromRoute] int reservationId)
    {
        var userId = int.Parse(User.Claims.First(c => c.Type == ClaimTypes.NameIdentifier).Value); 
        await _reservationService.CancelReservation(reservationId);
        return NoContent();
    }

    [HttpGet("/reservations/{userId:int}")]
    public async Task<ActionResult<List<ReservationDto>>> GetReservationsForUser([FromRoute] int userId)
    {
        var reservationList = await _reservationService.GetReservationsForUser(userId);
        return Ok(reservationList);
    }
}