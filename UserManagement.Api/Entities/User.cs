﻿using System.ComponentModel.DataAnnotations;

namespace UserManagement.Api.Entities;

public class User
{
    [Key]
    public Guid Guid { get; set; }
    public required string Login { get; set; }
    public required string Password { get; set; }
    public required string Name { get; set; }
    public int Gender { get; set; }
    public DateTime? Birthday { get; set; }
    public bool Admin { get; set; }
    public DateTime CreatedOn { get; set; }
    public required string CreatedBy { get; set; }
    public DateTime ModifiedOn { get; set; }
    public required string ModifiedBy { get; set; }
    public DateTime? RevokedOn { get; set; }
    public string? RevokedBy { get; set; }
}
