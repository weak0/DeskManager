using DeskManager.Models;
using DeskManager.Services;
using DeskManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskManager.Controllers;

[ApiController]
[Route("/desks")]
[Authorize]
public class DeskController : ControllerBase
{
    private readonly IDeskService _deskService;

    public DeskController(IDeskService deskService)
    {
        _deskService = deskService;
    }
    [HttpGet("/desks/locations/{locationId:int}")]
    public async Task<ActionResult<List<GetDeskDto>>> GetDesks([FromRoute] int locationId)
    {
        var isAdmin = AuthService.IsAdmin(User.Claims);
        var desks =await  _deskService.GetDesks(locationId, isAdmin);
        return Ok(desks);
    }

    [HttpGet("/desks/{deskId:int}")]
    public async Task<ActionResult<GetDeskDto>> GetDesk([FromRoute] int deskId)
    {
        var isAdmin = AuthService.IsAdmin(User.Claims);
        var desk = await _deskService.GetDesk(deskId, isAdmin);
        return Ok(desk);
    }

    [HttpPost("/desks/locations/{locationId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ModifyDeskDto>> CreateDesk([FromRoute] int locationId)
    {
        var desk = await _deskService.CreateDesk(locationId);
        return Created($"/{locationId}/desks/{desk.Id}", desk);
    }

    [HttpPut("{deskId:int}/locations/{locationId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ModifyDeskDto>> UpdateDesk([FromRoute] int locationId, [FromRoute] int deskId)
    {
        var desk = await _deskService.UpdateDeskLocation(locationId, deskId);
        return Ok(desk);
    }

    [HttpDelete("/desks/{deskId:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteDesk([FromRoute] int deskId)
    {
        await _deskService.DeleteDesk(deskId);
        return NoContent();
    }

    [HttpPatch("/desks/{deskId:int}/unavailable")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<ModifyDeskDto>> ChangeDeskAvailable([FromRoute] int deskId)
    {
        var desk = await _deskService.MakeDeskUnavailable(deskId);
        return Ok(desk);
    }

}