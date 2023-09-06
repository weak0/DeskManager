namespace DeskManager.Models;

public class DeskReservationDto
{
    public DateTime StartDate { get; set; }
    public DateTime EndDate  { get; set; }
    public string UserEmail { get; set; }
}