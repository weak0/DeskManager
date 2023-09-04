using System.ComponentModel.DataAnnotations;

namespace DeskManager.Models;

public class LoginDto
{
    [EmailAddress]
    [Required]
    public string Email { get; set; }
    [MinLength(6)]
    [Required]
    public string Password { get; set; }
}