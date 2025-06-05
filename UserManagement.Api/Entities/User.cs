using System.ComponentModel.DataAnnotations;

namespace UserManagement.Api.Entities;

/// <summary>
/// Represents a user
/// </summary>
public class User
{
    /// <summary>
    /// User's ID.
    /// </summary>
    [Key]
    public Guid Guid { get; set; }
    /// <summary>
    /// User's login.
    /// </summary>
    public required string Login { get; set; }
    /// <summary>
    /// Hash of the user's password.
    /// </summary>
    public required string Password { get; set; }
    /// <summary>
    /// User's name.
    /// </summary>
    public required string Name { get; set; }
    /// <summary>
    /// User's gender, 0 - female, 1 - male, 2 - unknown.
    /// </summary>
    public int Gender { get; set; }
    /// <summary>
    /// User's date of birth.
    /// </summary>
    public DateTime? Birthday { get; set; }
    /// <summary>
    /// Is this user an admin?
    /// </summary>
    public bool Admin { get; set; }
    /// <summary>
    /// Date of the user's creation.
    /// </summary>
    public DateTime CreatedOn { get; set; }
    /// <summary>
    /// Login of the user responsible for creating this user record.
    /// </summary>
    public required string CreatedBy { get; set; }
    /// <summary>
    /// Date of the last modification made to this record.
    /// </summary>
    public DateTime ModifiedOn { get; set; }
    /// <summary>
    /// Login of the user responsible for modifying this user record.
    /// </summary>
    public required string ModifiedBy { get; set; }
    /// <summary>
    /// Date on which this user had their access revoked; null indicates that this user's access has not been revoked.
    /// </summary>
    public DateTime? RevokedOn { get; set; }
    /// <summary>
    /// Login of the user responsible for revoking access from this user.
    /// </summary>
    public string? RevokedBy { get; set; }
}
