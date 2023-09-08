using DeskManager.Entities;
using DeskManager.Models;

namespace DeskManager.Services.Interfaces;

public interface IReservationService
{
        Task<ReservationDto> ShortReservation(int deskId, CreateShortReservationDto reservation);
        Task<ReservationDto> MakeReservation(int deskId, CreateReservationDto createReservation); 
        Task CancelReservation(int reservationId);
        Task<List<ReservationDto>> GetReservationsForUser(int userId);

}