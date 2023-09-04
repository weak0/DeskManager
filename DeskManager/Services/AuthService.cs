using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DeskManager.Services;

public class AuthService : IAuthService
{
    private readonly DeskManagerDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IValidator<CreateUserDto> _userValidator;

    public AuthService(DeskManagerDbContext dbContext, IPasswordHasher<User> passwordHasher, IValidator<CreateUserDto> userValidator)
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _userValidator = userValidator;
    }
    public async Task CreateUser(CreateUserDto newUser)
    {
        var validationResult = await _userValidator.ValidateAsync(newUser);

        if (!validationResult.IsValid)
        {
            var errors = string.Join(Environment.NewLine, validationResult.Errors);
            throw new RegistrationValidationException($"Registration fault :{errors} ");
        }
        var user = MapUser(newUser);
        await _dbContext.Users.AddAsync(user);
        await _dbContext.SaveChangesAsync();
    }

    private User MapUser(CreateUserDto newUser)
    {
        var user = new User()
        {
            FirstName = newUser.FirstName,
            LastName = newUser.LastName,
            Email = newUser.Email,
            RoleId = 2
        };
        var hashPassword = _passwordHasher.HashPassword(user, newUser.Password);
        user.Password = hashPassword;
        return user;
    }
}

