﻿namespace DeskManager.Entities;

public class Location
{
    public int Id { get; set; }
    public string Name { get; set; }
    public string Room { get; set; }
    public List<Desk> Desks { get; set; } = new List<Desk>();
    public string City { get; set; }
    public string Street { get; set; }
}