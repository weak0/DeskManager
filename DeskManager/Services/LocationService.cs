using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Models.Mappers;
using DeskManager.Models.Validators;
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
    public async Task<List<LocationDto>> GetAll()
    {
        var locations = await _dbContext.Locations
            .Include(l => l.Desks)
            .ToListAsync();
        return locations.Select(LocationMapper.LocationToLocationDto).ToList();
    }

    public async Task<LocationDto> GetLocation(int id)
    {
        var location = await GetById(id);
        return LocationMapper.LocationToLocationDto(location);
    }

    public async Task<LocationDto> CreateLocation( CreateLocationDto createLocationDto)
    {
        var validationResult = await _createLocationValidation.ValidateAsync(createLocationDto);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(Environment.NewLine, validationResult.Errors);
            throw new WrongDataException($"Create location failed: {errors}");
        }

        var location = LocationMapper.CreateLocationDtoToLocation(createLocationDto);
        await _dbContext.Locations.AddAsync(location);
        await _dbContext.SaveChangesAsync();
        return LocationMapper.LocationToLocationDto(location);
    }

    public async Task DeleteLocation(int id)
    {
        var location = await GetById( id);
        if (location.Desks.Any())
            throw new InvalidOperationException("this location is not empty");
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