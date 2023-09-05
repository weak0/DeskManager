namespace DeskManager.Models;

public class ReservationDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    
    public int UserId { get; set; }
}