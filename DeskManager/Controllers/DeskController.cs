using System.Security.Claims;
using DeskManager.Entities;
using DeskManager.Models;
using DeskManager.Services;
using DeskManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskManager.Controllers;

[ApiController]
[Route("/locations/{locationId:int}/desks")]
public class DeskController : ControllerBase
{
    private readonly IDeskService _deskService;

    public DeskController(IDeskService deskService)
    {
        _deskService = deskService;
    }
    [HttpGet]
    public async Task<ActionResult<Desk>> GetDesks([FromRoute] int locationId)
    {
        var isAdmin = AuthService.IsAdmin(User.Claims);
        var desks =await  _deskService.GetDesks(locationId, isAdmin);
        return Ok(desks);
    }

    [HttpGet("/locations/desks/{deskId:int}")]
    public async Task<ActionResult<Desk>> GetDesk([FromRoute] int deskId)
    {
        var isAdmin = AuthService.IsAdmin(User.Claims);
        var desk = await _deskService.GetDesk(deskId, true);
        return Ok(desk);
    }

    [HttpPost]
    public async Task<ActionResult<Desk>> CreateDesk([FromRoute] int locationId)
    {
        var desk = await _deskService.CreateDesk(locationId);
        return Created($"/{locationId}/desks/{desk.Id}", desk);
    }

    [HttpPatch("{deskId:int}")]
    public async Task<ActionResult<Desk>> UpdateDesk([FromRoute] int locationId, [FromRoute] int deskId)
    {
        var desk = await _deskService.UpdateDeskLocation(locationId, deskId);
        return Ok(desk);
    }

    [HttpDelete("/locations/desks/{deskId:int}")]
    public async Task<ActionResult> DeleteDesk([FromRoute] int deskId)
    {
        await _deskService.DeleteDesk(deskId);
        return NoContent();
    }

}