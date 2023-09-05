using System.ComponentModel.DataAnnotations;

namespace DeskManager.Entities;

public class Desk
{
    public int Id { get; set; }
    
    public Location Location { get; set; }
    public int LocationId { get; set; }

    public List<Reservation> Reservations { get; set; }
    public bool IsAvailable { get; set; } = true;

    
}