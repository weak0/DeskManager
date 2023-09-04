using DeskManager.Entities;
using DeskManager.Models;

namespace DeskManager.Services.Interfaces;

public interface ILocationService
{
    Task<List<Location>> GetAll();
    Task<Location> GetLocation(int id);
    Task<int> CreateLocation(CreateLocationDto locationName);
    Task DeleteLocation(int id);
    Task<Location> GetById(int id);
}