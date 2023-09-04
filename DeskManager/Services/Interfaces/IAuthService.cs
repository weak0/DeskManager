using DeskManager.Models;

namespace DeskManager.Services.Interfaces;

public interface IAuthService
{
     Task CreateUser(CreateUserDto newUser);
     Task<string> Login(LoginDto user);
}