using System.ComponentModel;
using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Models.Mappers;
using DeskManager.Models.Validators;
using DeskManager.Services.Interfaces;
using Microsoft.EntityFrameworkCore;


namespace DeskManager.Services;

public class DeskService : IDeskService
{
    private readonly DeskManagerDbContext _dbContext;
    private readonly ILocationService _locationService;

    public DeskService(DeskManagerDbContext dbContext, ILocationService locationService)
    {
        _dbContext = dbContext;
        _locationService = locationService;
    }
    public async Task<List<GetDeskDto>> GetDesks(int locationId, bool isAdmin)
    {
        var desks = await GetListDesksQuery( locationId, isAdmin);
        return desks.Select(DeskMapper.DeskToGetDeskDto).ToList();
    }

    public async Task<GetDeskDto> GetDesk(int deskId, bool isAdmin)
    {
        var desk = await GetDeskQuery( deskId, isAdmin);
        return DeskMapper.DeskToGetDeskDto(desk);
    }

    public async Task<ModifyDeskDto> CreateDesk(int locationId)
    {
        await _locationService.GetById(locationId);

        var desk = new Desk()
        {
            LocationId = locationId
        };

        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();
        return DeskMapper.DeskToModifyDeskDto(desk);

    }

    public async Task<ModifyDeskDto> UpdateDeskLocation( int newLocationId, int deskId)
    {
        await _locationService.GetById(newLocationId);
        var desk = await GetDeskQuery(deskId, false);
        desk.LocationId = newLocationId;
        await _dbContext.SaveChangesAsync();
        return DeskMapper.DeskToModifyDeskDto(desk);
    }

    public async Task DeleteDesk(int deskId)
    {
        var desk = await GetDeskQuery(deskId, false);
        _dbContext.Desks.Remove(desk);
        await _dbContext.SaveChangesAsync();

    }

    public async Task<ModifyDeskDto> MakeDeskUnavailable(int deskId)
    {
        var desk = await GetDeskQuery(deskId, false);
        if (desk.Reservations.Any())
            throw new WarningException("This desk has reservations");
        desk.IsAvailable = false;
        await _dbContext.SaveChangesAsync();
        return DeskMapper.DeskToModifyDeskDto(desk);

    }
    public async Task<Desk> GetDeskQuery(int deskId, bool withUsers)
    {
        var baseQuery = BaseQuery(withUsers);
            var desk = await baseQuery.FirstOrDefaultAsync(d => d.Id == deskId) 
                       ?? throw new NotFoundException("Desk not found");
        return desk;
    }

    private async Task<List<Desk>> GetListDesksQuery(int locationId, bool withUsers)
    {
        var baseQuery = BaseQuery(withUsers);
        var desks = await baseQuery
            .Where(d => d.LocationId == locationId)
            .ToListAsync();
        if (!desks.Any())
        {
            throw new NotFoundException("Locations dont exists or is empty");
        }   
        return desks;
    }


    private IQueryable<Desk> BaseQuery(bool withUsers)
    {
        var baseQuery = _dbContext.Desks.AsQueryable();

        baseQuery = baseQuery.Include(d => d.Location);
        if (withUsers)
        {
            baseQuery = baseQuery.Include(d => d.Reservations)
                .ThenInclude(r => r.User);
        }
        else
        {
            baseQuery = baseQuery.Include(d => d.Reservations);
        }

        return baseQuery;
    }

}