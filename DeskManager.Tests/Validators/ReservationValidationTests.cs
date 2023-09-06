using DeskManager.Models;
using DeskManager.Models.Validators;

namespace DeskManager.Tests.Validators;

public class ReservationValidationTests
{
    
    [Theory]
    [InlineData(  "2024-09-01", "2024-09-06")]
    [InlineData(  "2024-09-01", "2024-09-07")]
    [InlineData(  "2024-09-01", "2024-09-08")]
    public void ValidReservationDto_PassValidation_IsValid(string startDate, string endDate)
    {
        //Arrange
        var reservationDto = new ReservationDto()
        {
            StartDate = DateTime.Parse(startDate),
            EndDate = DateTime.Parse(endDate)
        };
        var validator = new ReservationValidation();
        //Act
        var result = validator.Validate(reservationDto);
        //Assert
        Assert.True(result.IsValid);
        
    }
    [Theory]
    [InlineData(  "2024-09-01", "2024-10-08")]
    [InlineData(  "2024-10-08", "2024-09-01")]
    [InlineData( "2023-05-06", "2023-05-07")]
    public void ValidReservationDto_PassValidation_IsNotValid(string startDate, string endDate)
    {
        //Arrange
        var reservationDto = new ReservationDto()
        {
            StartDate = DateTime.Parse(startDate),
            EndDate = DateTime.Parse(endDate) 
        };
        var validator = new ReservationValidation();
        //Act
        var result = validator.Validate(reservationDto);
        //Assert
        Assert.False(result.IsValid);
        
    }
}