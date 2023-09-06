using DeskManager.Entities;
using DeskManager.Models.Validators;

namespace DeskManager.Models.Mappers;

public static class DeskMapper
{
    public static GetDeskDto DeskToGetDeskDto(Desk desk)
    {
          var reservations = desk.Reservations.Select(r => new DeskReservationDto()
            {
                StartDate = r.StartDate,
                EndDate = r.EndDate,
                UserEmail = r.User?.Email 
            
            }).ToList();
            var result = new GetDeskDto()
            {
                Id = desk.Id,
                LocationName = desk.Location.Name,
                IsAvailable = desk.IsAvailable,
                NumberOfReservations = reservations.Count,
                Reservations = reservations
            };
            return result;
    }

    public static ModifyDeskDto DeskToModifyDeskDto(Desk desk)
    {
        var result = new ModifyDeskDto()
        {
            Id = desk.Id,
            IsAvailable = desk.IsAvailable,
            LocationId = desk.LocationId
        };
        return result;
    }
}