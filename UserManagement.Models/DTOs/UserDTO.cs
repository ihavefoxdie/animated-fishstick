using System;

namespace UserManagement.Models.DTOs;

public class UserDTO
{
    public required string Name { get; set; }
    public int Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public DateTime? RevokedOn { get; set; }
}
