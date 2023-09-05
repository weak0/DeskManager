using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeskManager.Services;

public class ReservationService : IReservationService
{
    private readonly DeskManagerDbContext _dbContext;
  
    private readonly IValidator<ReservationDto> _reservationValidator;
    private readonly IDeskService _deskService;

    public ReservationService(DeskManagerDbContext dbContext, IValidator<ReservationDto> reservationValidator, IDeskService deskService)
    {
        _dbContext = dbContext;
        _reservationValidator = reservationValidator;
        _deskService = deskService;
    }

    public async Task<Desk> MakeReservation(int deskId, ReservationDto reservation)
    {
        var desk = await _deskService.GetDeskQuery(deskId, false);
        IsReservationOverlapping(desk, reservation);
        
        var validationResult = await _reservationValidator.ValidateAsync(reservation);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(Environment.NewLine, validationResult.Errors);
            throw new WrongDataException($"Registration fault :{errors} ");
        }

        var newReservation = new Reservation()
        {
            StartDate = reservation.StartDate,
            EndDate = reservation.EndDate,
            UserId = reservation.UserId,
            DeskId = deskId,
        };
        _dbContext.Reservations.Add(newReservation);
        await _dbContext.SaveChangesAsync();
        return desk;

    }

    public async Task<Desk> CancelReservation(int deskId, int reservationId)
    {
        var desk = await _deskService.GetDeskQuery(deskId, true);
        
        var userPreviousReservation = desk.Reservations.FirstOrDefault(r => r.Id == reservationId) 
                                      ?? throw new WrongDataException("This desk dont have yours reservation");
        
        if (userPreviousReservation.StartDate.AddDays(-1) < DateTime.Now)
            throw new WrongDataException("You cant cancel reservation for this desk");

        _dbContext.Reservations.Remove(userPreviousReservation);
        await _dbContext.SaveChangesAsync();
        return desk;
    }

    private static void IsReservationOverlapping(Desk desk, ReservationDto dto)
    {
        var overlappingReservations = desk.Reservations
            .Where(r =>
                (dto.StartDate >= r.StartDate && dto.StartDate <= r.EndDate) ||
                (dto.EndDate >= r.StartDate && dto.EndDate <= r.EndDate))
            .ToList();
        if (overlappingReservations.Any())
            throw new WrongDataException("Sorry in chosen time this desk is unavailable");
    }
    
}