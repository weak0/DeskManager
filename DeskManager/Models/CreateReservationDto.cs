namespace DeskManager.Models;

public class CreateReservationDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public int UserId { get; set; }
}