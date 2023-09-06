using DeskManager.Entities;

namespace DeskManager.Models.Mappers;

public static class ReservationMapper
{
    public static Reservation CreateReservationDtoToReservation(CreateReservationDto dto)
    {
        var result = new Reservation()
        {
            StartDate = dto.StartDate,
            EndDate = dto.EndDate,
            UserId = dto.UserId
        };
        return result;
    }

    public static ReservationDto ReservationToReservationDto(Reservation reservation)
    {
        var result = new ReservationDto()
        {
            DeskId = reservation.DeskId,
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
        };
        return result;
    }

    public static CreateReservationDto ShortReservationToCreateReservationDto(CreateShortReservationDto dto)
    {
        var result = new CreateReservationDto()
        {
            StartDate = dto.StartDate,
            EndDate = dto.StartDate.AddDays(1),
            UserId = dto.UserId
        };
        return result;
    }
}