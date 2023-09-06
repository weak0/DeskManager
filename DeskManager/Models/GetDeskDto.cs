using DeskManager.Entities;

namespace DeskManager.Models;

public class GetDeskDto
{
    public int  Id { get; set; }
    public string LocationName { get; set; }
    public bool IsAvailable { get; set; }
    public int NumberOfReservations { get; set; }
    public List<DeskReservationDto> Reservations { get; set; }
    
} 