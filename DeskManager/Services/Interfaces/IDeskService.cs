using System.Linq.Expressions;
using DeskManager.Entities;
using DeskManager.Models;
using DeskManager.Models.Validators;

namespace DeskManager.Services.Interfaces;

public interface IDeskService
{
    Task<List<GetDeskDto>> GetDesks(int locationId, bool isAdmin);
    Task<GetDeskDto> GetDesk(int deskId, bool isAdmin);
    Task<ModifyDeskDto> CreateDesk( int locationId);
    Task<ModifyDeskDto> UpdateDeskLocation( int newLocationId, int deskId);
    Task DeleteDesk(int deskId);
    Task<Desk> GetDeskQuery(int deskId, bool withUsers);
    Task<ModifyDeskDto> MakeDeskUnavailable(int deskId);
}