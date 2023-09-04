using DeskManager.Entities;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace DeskManager.Controllers;

[ApiController]
[Route("auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }
    [HttpPost]
    public async Task<ActionResult> CreateUser([FromBody]CreateUserDto newUser)
    {
        await _authService.CreateUser(newUser);
        return Ok(newUser.Email);
    }
}