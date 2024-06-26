﻿using API.Extensions;

namespace API.Entities;

public class AppUser
{
    public int Id { get; set; }
    public string UserName { get; set; }
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public DateOnly DateOfBirth { get; set; }
    public string KnownAs { get; set; }
    public DateTime Created { get; set; } = DateTime.UtcNow;
    public DateTime LastActive { get; set; } = DateTime.UtcNow;
    public string City { get; set; }
    public string Country { get; set; }
    public List<Entry> Entries { get; set; } = new();

    /*public int GetAge()
    {
        return DateOfBirth.CalculateAge();
    }*/
}
