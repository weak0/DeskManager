using DeskManager.Entities;
using DeskManager.Models;

namespace DeskManager.Services.Interfaces;

public interface IReservationService
{
        Task<Desk> MakeReservation(int deskId, ReservationDto reservation); 
        Task<Desk> CancelReservation(int deskId, int reservationId);
}