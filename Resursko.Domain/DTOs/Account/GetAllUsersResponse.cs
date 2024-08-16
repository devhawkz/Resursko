﻿namespace Resursko.Domain.DTOs.Account;

public record class GetAllUsersResponse
{
    public string? Id { get; set; }
    public string? FirstName { get; set; }
    public string? LastName { get; set; }
    public string? Email { get; set; }
    public string? Username { get; set; }
    public List<string> Roles { get; set; } = new List<string>();
    public int ActiveReservations { get; set; } = 0;
}
