using System;

namespace UserManagement.Models.DTOs;

public class UserDTO
{
    public required string Name { get; init; }
    public int Gender { get; init; }
    public DateTime? Birthday { get; init; }
    public bool Active { get; init; } = true;
}
