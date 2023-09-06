using DeskManager.Entities;
using DeskManager.Models;
using DeskManager.Models.Validators;
using DeskManager.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DeskManager.Controllers;

[ApiController]
[Route("locations")]
[Authorize]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }
    [HttpGet]
    public async Task<ActionResult<List<LocationDto>>> GetAll()
    {
        var locations = await _locationService.GetAll();
        return Ok(locations);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<LocationDto>> GetLocation([FromRoute] int id)
    {
        var location = await _locationService.GetLocation(id);
        return Ok(location);
    }

    [HttpPost]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> CreateLocation([FromBody] CreateLocationDto locationName)
    {
        var location = await _locationService.CreateLocation(locationName);
        return Created($"/locations/{location.Id}", location);
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> DeleteLocation([FromRoute] int id)
    {
        await _locationService.DeleteLocation(id);
        return NoContent();
    }
}