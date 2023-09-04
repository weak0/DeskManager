using System.Linq.Expressions;
using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using FluentValidation;
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
    public async Task<List<Desk>> GetDesks(int locationId, bool isAdmin)
    {
        var includes = isAdmin
            ? new Expression<Func<Desk, object>>[] { d => d.User }
            : null;
        return await GetAllDesksQuery( locationId, includes);
    }

    public async Task<Desk> GetDesk(int deskId, bool isAdmin)
    {
        var includes = isAdmin
            ? new Expression<Func<Desk, object>>[] { d => d.User }
            : null;
        return await GetDeskQuery( deskId, includes);

    }

    public async Task<Desk> CreateDesk(int locationId)
    {
        await _locationService.GetById(locationId);

        var desk = new Desk()
        {
            LocationId = locationId
        };

        await _dbContext.Desks.AddAsync(desk);
        await _dbContext.SaveChangesAsync();
        return desk;

    }

    public async Task<Desk> UpdateDeskLocation( int newLocationId, int deskId)
    {
        await _locationService.GetById(newLocationId);
        var desk = await GetDeskQuery(deskId, null);
        desk.LocationId = newLocationId;
        await _dbContext.SaveChangesAsync();
        return desk;
    }

    public async Task DeleteDesk(int deskId)
    {
        var desk = await GetDeskQuery(deskId, null);
        _dbContext.Desks.Remove(desk);
        await _dbContext.SaveChangesAsync();

    }
    private async Task<Desk> GetDeskQuery(int deskId, Expression<Func<Desk, object>>[] includes)
    {
        var baseQuery = BaseQuery(includes);
            var desk = await baseQuery.FirstOrDefaultAsync(d => d.Id == deskId) 
                       ?? throw new NotFoundException("Desk not found");
        return desk;
    }

    private async Task<List<Desk>> GetAllDesksQuery(int locationId, Expression<Func<Desk, object>>[] includes)
    {
        var baseQuery = BaseQuery(includes);
        var desks = await baseQuery
            .Where(d => d.LocationId == locationId)
            .ToListAsync();
        if (!desks.Any())
        {
            throw new NotFoundException("Locations dont exists or is empty");
        }   
        return desks;
    }

    private IQueryable<Desk> BaseQuery(Expression<Func<Desk, object>>[] includes)
    {
        var baseQuery = _dbContext.Desks.AsQueryable();
        if (includes != null)
        {
            baseQuery = includes
                .Aggregate(baseQuery, (current, include) => current.Include(include));
        }

        return baseQuery;
    }
}