using DeskManager.Entities;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskManager.Controllers;

[ApiController]
[Route("locations")]
public class LocationController : ControllerBase
{
    private readonly ILocationService _locationService;

    public LocationController(ILocationService locationService)
    {
        _locationService = locationService;
    }
    [HttpGet]
    public async Task<ActionResult<List<Location>>> GetAll()
    {
        var locations = await _locationService.GetAll();
        return Ok(locations);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Location>> GetLocation([FromRoute] int id)
    {
        var location = await _locationService.GetLocation(id);
        return Ok(location);
    }

    [HttpPost]
    public async Task<ActionResult> CreateLocation([FromBody] CreateLocationDto locationName)
    {
        var locationId = await _locationService.CreateLocation(locationName);
        return Created($"/locations/{locationId}", null);
    }

    [HttpDelete("{id:int}")]
    public async Task<ActionResult> DeleteLocation([FromRoute] int id)
    {
        await _locationService.DeleteLocation(id);
        return NoContent();
    }
}