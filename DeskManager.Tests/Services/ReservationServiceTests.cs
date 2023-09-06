using DeskManager.Entities;
using DeskManager.Exceptions;
using DeskManager.Models;
using DeskManager.Services;
using DeskManager.Services.Interfaces;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace DeskManager.Tests.Services;

public class ReservationServiceTests
{
    [Fact]
    public void IsReservationOverlapping_IsOverlapping_ThrowsWrongDataException()
    {
        // Arrange
        var desk = new Desk()
        {
            Reservations = new List<Reservation>()
            {
                new Reservation() {StartDate = DateTime.Now.AddDays(-2), EndDate = DateTime.Now.AddDays(2)}
            }
        };

        var reservationDto = new ReservationDto()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3)
        };
        //Act && Assert
        Assert.Throws<WrongDataException>(() => ReservationService.IsReservationOverlapping(desk, reservationDto));
    }
    
    [Fact]
    public void IsReservationOverlapping_IsNotOverlapping()
    {
        // Arrange
        var desk = new Desk()
        {
            Reservations = new List<Reservation>()
            { }
        };

        var reservationDto = new ReservationDto()
        {
            StartDate = DateTime.Now,
            EndDate = DateTime.Now.AddDays(3)
        };
        //Act && Assert
        ReservationService.IsReservationOverlapping(desk, reservationDto);
    }
    
}


