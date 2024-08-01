﻿using Microsoft.AspNetCore.Identity;

namespace Resursko.Domain.Models;

public class Role : IdentityRole
{
    public string? Description { get; set; }
}
