using DeskManager.Entities;
using DeskManager.Models;
using DeskManager.Models.Validators;

namespace DeskManager.Services.Interfaces;

public interface ILocationService
{
    Task<List<LocationDto>> GetAll();
    Task<LocationDto> GetLocation(int id);
    Task<LocationDto> CreateLocation(CreateLocationDto locationName);
    Task DeleteLocation(int id);
    Task<Location> GetById(int id);
}