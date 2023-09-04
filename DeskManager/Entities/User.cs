﻿namespace DeskManager.Entities;

public class User
{
    public int Id { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Role Role { get; set; }
    public int RoleId { get; set; }
    public Desk Desk { get; set; }
    public int? DeskId { get; set; }
}