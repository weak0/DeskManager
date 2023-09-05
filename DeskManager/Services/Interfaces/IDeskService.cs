using System.Linq.Expressions;
using DeskManager.Entities;
using DeskManager.Models;

namespace DeskManager.Services.Interfaces;

public interface IDeskService
{
    Task<List<Desk>> GetDesks(int locationId, bool isAdmin);
    Task<Desk> GetDesk(int deskId, bool isAdmin);
    Task<Desk> CreateDesk( int locationId);
    Task<Desk> UpdateDeskLocation( int newLocationId, int deskId);
    Task DeleteDesk(int deskId);
    Task<Desk> GetDeskQuery(int deskId, bool withUsers);
    Task MakeDeskUnavailable(int deskId);
}