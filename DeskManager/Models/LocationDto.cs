namespace DeskManager.Models.Validators;

public class LocationDto
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Room { get; set; }
    public int NumberOfDesks { get; set; } 
    public string City { get; set; }
    public string Street { get; set; }
}