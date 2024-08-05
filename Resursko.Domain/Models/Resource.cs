﻿namespace Resursko.Domain.Models;
public class Resource
{
    public int Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public bool IsAvailable { get; set; } = true;

    public List<Reservation> Reservations { get; set; } = new List<Reservation>();

}
