using System;
using System.ComponentModel.DataAnnotations;

namespace UserManagement.Models.DTOs;

public record UserDTO(string Name, int Gender, DateTime? Birthday, bool Active);
