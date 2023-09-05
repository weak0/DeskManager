using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeskManager.Services;

public class LocationService : ILocationService
{
    private readonly DeskManagerDbContext _dbContext;
    private readonly IValidator<CreateLocationDto> _createLocationValidation;

    public LocationService(DeskManagerDbContext dbContext, IValidator<CreateLocationDto> createLocationValidation)
    {
        _dbContext = dbContext;
        _createLocationValidation = createLocationValidation;
    }
    public async Task<List<Location>> GetAll()
    {
        var locations = await _dbContext.Locations
            .Include(l => l.Desks)
            .ToListAsync();
        return locations;
    }

    public async Task<Location> GetLocation(int id)
    {
        var location = await GetById(id);
        return location;
    }

    public async Task<int> CreateLocation( CreateLocationDto locationName)
    {
        var validationResult = await _createLocationValidation.ValidateAsync(locationName);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(Environment.NewLine, validationResult.Errors);
            throw new WrongDataException($"Create location failed: {errors}");
        }
        var location = new Location()
        {
            Name = locationName.Name
        };
        await _dbContext.Locations.AddAsync(location);
        await _dbContext.SaveChangesAsync();
        return location.Id;
    }

    public async Task DeleteLocation(int id)
    {
        var location = await GetById( id);
        _dbContext.Locations.Remove(location);
        await _dbContext.SaveChangesAsync();

    }
    public async Task<Location> GetById(int id)
    {
        var location = await _dbContext.Locations
                           .Include(l => l.Desks)
                           .FirstOrDefaultAsync(l => l.Id == id) 
                       ?? throw new NotFoundException("Location not found");
        return location;
    }
    
}