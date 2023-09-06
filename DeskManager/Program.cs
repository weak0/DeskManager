using System.Security.Claims;
using System.Text;
using System.Text.Json.Serialization;
using DeskManager;
using DeskManager.Entities;
using DeskManager.Middleware;
using DeskManager.Models;
using DeskManager.Models.Validators;
using DeskManager.Services;
using DeskManager.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers().AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);;
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// JWT CONFIG
var authSettings = new AuthenticationSettings();
builder.Configuration.GetSection("Auth").Bind(authSettings);
builder.Services.AddSingleton(authSettings);
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = "Bearer";
    options.DefaultScheme = "Bearer";
    options.DefaultChallengeScheme = "Bearer";
}).AddJwtBearer(options => 
{
    options.RequireHttpsMetadata = false;
    options.SaveToken = true;
    options.TokenValidationParameters = new TokenValidationParameters()
    {
        ValidIssuer = authSettings.JwtIssuer,
        ValidAudience = authSettings.JwtIssuer,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(authSettings.JwtKey))
    };
});

builder.Services.AddDbContext<DeskManagerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnect"));
});

builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidation>();
builder.Services.AddScoped<IValidator<CreateLocationDto>, CreateLocationValidation>();
builder.Services.AddScoped<IValidator<CreateReservationDto>, ReservationValidation>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<ILocationService, LocationService>();
builder.Services.AddScoped<IDeskService, DeskService>();
builder.Services.AddScoped<IReservationService, ReservationService>();



var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseMiddleware<ErrorHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();