﻿using System.ComponentModel;
using System.Diagnostics;
using System.Linq.Expressions;
using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Models.Mappers;
using DeskManager.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeskManager.Services;

public class ReservationService : IReservationService
{
    private readonly DeskManagerDbContext _dbContext;
  
    private readonly IValidator<CreateReservationDto> _reservationValidator;
    private readonly IDeskService _deskService;

    public ReservationService(DeskManagerDbContext dbContext, IValidator<CreateReservationDto> reservationValidator, IDeskService deskService)
    {
        _dbContext = dbContext;
        _reservationValidator = reservationValidator;
        _deskService = deskService;
    }

    public async Task<ReservationDto> ShortReservation(int deskId, CreateShortReservationDto reservation)
    {
        var mapToReservationDto = ReservationMapper.ShortReservationToCreateReservationDto(reservation);
        var desk  = await MakeReservation(deskId, mapToReservationDto);
        return desk;
    }

    public async Task<ReservationDto> MakeReservation(int deskId, CreateReservationDto createReservation)
    {
        var desk = await _deskService.GetDeskQuery(deskId, false);
        if (!desk.IsAvailable)
            throw new WrongDataException("this desk is unavailable");
        
        IsReservationOverlapping(desk, createReservation);
        
        var validationResult = await _reservationValidator.ValidateAsync(createReservation);
        if (!validationResult.IsValid)
        {
            var errors = string.Join(Environment.NewLine, validationResult.Errors);
            throw new WrongDataException($"Registration fault :{errors} ");
        }

        var newReservation = ReservationMapper.CreateReservationDtoToReservation(createReservation);
        newReservation.DeskId = desk.Id;
        _dbContext.Reservations.Add(newReservation);
        await _dbContext.SaveChangesAsync();
        return ReservationMapper.ReservationToReservationDto(newReservation);

    }

    public async Task CancelReservation( int reservationId)
    {
        var userPreviousReservation = _dbContext.Reservations.FirstOrDefault(r => r.Id == reservationId) 
                                      ?? throw new WrongDataException("This desk dont have yours reservation");
        
        if (userPreviousReservation.StartDate.AddDays(-1) < DateTime.Now)
            throw new WrongDataException("You cant cancel reservation for this desk");

        _dbContext.Reservations.Remove(userPreviousReservation);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<List<ReservationDto>> GetReservationsForUser(int userId)
    {
        var reservations = await _dbContext.Reservations.Where(r => r.UserId == userId).ToListAsync();
        return reservations.Select(ReservationMapper.ReservationToReservationDto).ToList();
    }

    public static void IsReservationOverlapping(Desk desk, CreateReservationDto dto)
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