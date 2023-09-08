namespace DeskManager.Models;

public class DeskReservationDto
{
    public int Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate  { get; set; }
    public string UserEmail { get; set; }
}