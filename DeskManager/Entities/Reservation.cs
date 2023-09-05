namespace DeskManager.Entities;

public class Reservation
{
    public int  Id { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public Desk Desk { get; set; }
    public int DeskId { get; set; }
    
    public User User { get; set; }
    public int UserId { get; set; }
}