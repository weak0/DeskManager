namespace DeskManager.Entities;

public class Desk
{
    public int Id { get; set; }
    public Location Location { get; set; }
    public int LocationId { get; set; }
    public DateTime? StartReservation { get; set; }
    public DateTime? EndReservation { get; set; }
    public bool IsAvailable { get; set; }
}