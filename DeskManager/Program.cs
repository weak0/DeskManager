using DeskManager.Entities;
using DeskManager.Middleware;
using DeskManager.Models;
using DeskManager.Models.Validators;
using DeskManager.Services;
using DeskManager.Services.Interfaces;
using FluentValidation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DeskManagerDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DbConnect"));
});
builder.Services.AddScoped<IPasswordHasher<User>, PasswordHasher<User>>();
builder.Services.AddScoped<IAuthService, AuthService>();
builder.Services.AddScoped<IValidator<CreateUserDto>, CreateUserValidation>();
builder.Services.AddScoped<ErrorHandlingMiddleware>();
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