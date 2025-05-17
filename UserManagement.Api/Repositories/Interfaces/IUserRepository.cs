using System.Linq.Expressions;

namespace UserManagement.Api.Repositories.Interfaces;

/// <summary>
/// User repository interface
/// </summary>
/// <typeparam name="T">Entity to use</typeparam>
public interface IUserRepository<T> : IDisposable
{
    /// <summary>
    /// Saves a new item to the database
    /// </summary>
    /// <param name="user">Item to save</param>
    /// <returns>A Task.</returns>
    Task Create(T user);
    /// <summary>
    /// Updates the specified object (if it exists)
    /// </summary>
    /// <param name="user">Item to update</param>
    /// <returns>Updated object.</returns>
    Task<T?> Update(T user);
    /// <summary>
    /// Gets every item that matches the query.
    /// </summary>
    /// <param name="exp">"Expression representing the query filter</param>
    /// <returns>A collection of items found.</returns>
    IEnumerable<T> ReadAll(Func<T, bool> exp);
    /// <summary>
    /// Gets a specific item that matches the query.
    /// </summary>
    /// <param name="exp">"Expression representing the query filter</param>
    /// <returns>Found item if successful, otherwise null.</returns>
    Task<T?> Read(Expression<Func<T, bool>> exp);
    /// <summary>
    /// Deletes the item specified
    /// </summary>
    /// <param name="user">Item to delete</param>
    /// <returns>Deleted item if successful, otherwise null.</returns>
    Task<T?> Delete(T user);
}
