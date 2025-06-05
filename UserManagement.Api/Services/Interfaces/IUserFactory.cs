using System;
using UserManagement.Api.Entities;

namespace UserManagement.Api.Services.Interfaces;

/// <summary>
/// Abstact factory interface for converting <see cref="User"/> entities to <typeparamref name="T"/>.
/// </summary>
/// <typeparam name="T">A type to convert to or from (for example, a DTO model).</typeparam>
public interface IUserFactory<T>
{
    /// <summary>
    /// Converts from a <typeparamref name="T"/> entity to <see cref="User"/>. 
    /// </summary>
    /// <param name="entity">A <see cref="User"/> entity to convert from.</param>
    /// <returns>New entity of the type <typeparamref name="T"/>.</returns>
    public T CreateModel(User entity);
}
