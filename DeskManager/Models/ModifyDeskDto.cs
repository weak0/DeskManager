namespace DeskManager.Models;

public class ModifyDeskDto
{
    public int Id { get; set; }
    public int LocationId { get; set; }
    public bool IsAvailable { get; set; }
}