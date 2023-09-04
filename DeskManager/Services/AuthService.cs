using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace DeskManager.Services;

public class AuthService : IAuthService
{
    private readonly DeskManagerDbContext _dbContext;
    private readonly IPasswordHasher<User> _passwordHasher;
    private readonly IValidator<CreateUserDto> _userValidator;
    private readonly AuthenticationSettings _authSettings;


    public AuthService(DeskManagerDbContext dbContext, IPasswordHasher<User> passwordHasher, IValidator<CreateUserDto> userValidator, AuthenticationSettings authSettings )
    {
        _dbContext = dbContext;
        _passwordHasher = passwordHasher;
        _userValidator = userValidator;
        _authSettings = authSettings;
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

    public async Task<string> Login(LoginDto dto)
    {
        var user = await _dbContext.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Email == dto.Email) ??
                   throw new NotFoundException("Wrong email or password");
        var comparePassword = _passwordHasher.VerifyHashedPassword(user, user.Password, dto.Password);
        if(comparePassword != PasswordVerificationResult.Success)
        {
            throw new NotFoundException("Wrong email or password");
        }
        var token = GenerateToken(user);
        return token;
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
    private string GenerateToken(User user)
    {
        var claims = new List<Claim>()
        {
            new Claim (ClaimTypes.NameIdentifier,  user.Id.ToString()),
            new Claim (ClaimTypes.Email,  user.Email),
            new Claim (ClaimTypes.Role,  user.Role.RoleName),
        };
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authSettings.JwtKey));
        var creed = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
        var expires = DateTime.Now.AddDays(_authSettings.ExpiresDate);
        var token = new JwtSecurityToken(
            issuer: _authSettings.JwtIssuer,
            audience: _authSettings.JwtIssuer,
            claims: claims,
            expires: expires,
            signingCredentials: creed
        );
        var tokenHandler = new JwtSecurityTokenHandler();
        return tokenHandler.WriteToken(token);
    }
}

