using UserManagement.Api.Entities;
using UserManagement.Api.Services.Interfaces;
using UserManagement.Models.DTOs;

namespace UserManagement.Api.Services;

/// <summary>
/// A factory for converting <see cref="User"/> entities to <see cref="UserDTO"/>.
/// </summary>
public class UserDTOFactory : IUserFactory<UserDTO>
{
    /// <inheritdoc />
    public UserDTO CreateModel(User entity)
    {
        return entity == null
            ? throw new ArgumentNullException(nameof(entity))
            : new UserDTO(
            entity.Name,
            entity.Gender,
            entity.Birthday,
            entity.RevokedOn == null ? true : false
        );
    }
}
