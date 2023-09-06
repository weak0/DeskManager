using DeskManager.Entities;
using DeskManager.Models.Validators;

namespace DeskManager.Models.Mappers;

public static class LocationMapper
{
    public static LocationDto LocationToLocationDto(Location location)
    {
        var result = new LocationDto()
        {
            Id = location.Id,
            Name = location.Name,
            Room = location.Room,
            NumberOfDesks = location.Desks.Count,
            City = location.City,
            Street = location.Street
        };
        return result;
    }

    public static Location CreateLocationDtoToLocation(CreateLocationDto locationDto)
    {
        var result = new Location()
        {
            Name = locationDto.Name,
            Room = locationDto.Room,
            City = locationDto.City,
            Street = locationDto.Street,
        };
        return result;
    }
}