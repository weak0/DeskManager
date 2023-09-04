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
    [HttpPost("register")]
    public async Task<ActionResult> CreateUser([FromBody]CreateUserDto newUser)
    {
        await _authService.CreateUser(newUser);
        return Ok(newUser.Email);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Login([FromBody] LoginDto user)
    {
      var token = await _authService.Login(user);
      return Ok(token);
    }
}