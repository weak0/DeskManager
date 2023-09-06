using System.Reflection.Metadata.Ecma335;

namespace DeskManager.Models;

public class ReservationDto
{
    public int DeskId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
}