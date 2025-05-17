using UserManagement.Api.Data;
using UserManagement.Api.Repositories.Interfaces;
using UserManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UserManagement.Api.Repositories;

/// <summary>
/// IUserRepository implementation with the User entity
/// </summary>
public class UserRepository : IUserRepository<User>, IDisposable
{
    private bool disposed = false;
    private readonly UserDbContext _userDbContext;

    /// <summary>
    /// UserRepository constructor
    /// </summary>
    /// <param name="userDbContext">DbContext to use</param>
    public UserRepository(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }



    /// <inheritdoc />
    public async Task Create(User user)
    {
        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();
    }

    /// <inheritdoc />
    protected virtual void Dispose(bool disposing)
    {
        if (!this.disposed)
        {
            if (disposing)
            {
                _userDbContext.Dispose();
            }
        }
        this.disposed = true;
    }

    /// <inheritdoc />
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    /// <inheritdoc />
    public IEnumerable<User> ReadAll(Func<User, bool> exp)
    {
        return _userDbContext.Users.Where(exp);
    }

    /// <inheritdoc />
    public async Task<User?> Read(Expression<Func<User, bool>> exp)
    {
        return await _userDbContext.Users.FirstOrDefaultAsync(exp, default);
    }

    /// <inheritdoc />
    public async Task<User?> Update(User user)
    {
        User? userToUpdate = await _userDbContext.Users.FirstOrDefaultAsync(x => x.Guid == user.Guid);

        if (userToUpdate != null)
        {
            userToUpdate.Login = user.Login;
            userToUpdate.Password = user.Password;
            userToUpdate.Name = user.Name;
            userToUpdate.Gender = user.Gender;
            userToUpdate.Birthday = user.Birthday;
            userToUpdate.Admin = user.Admin;
            userToUpdate.ModifiedOn = DateTime.UtcNow;
            userToUpdate.ModifiedBy = user.ModifiedBy;
            userToUpdate.RevokedOn = user.RevokedOn;
            userToUpdate.RevokedBy = user.RevokedBy;
            await _userDbContext.SaveChangesAsync();
        }

        return userToUpdate;
    }

    /// <inheritdoc />
    public async Task<User?> Delete(User user)
    {
        User? usetToDelete = await _userDbContext.Users.FirstOrDefaultAsync(x => x.Guid == user.Guid);

        if (usetToDelete != null)
        {
            _userDbContext.Remove(usetToDelete);
            await _userDbContext.SaveChangesAsync();
        }

        return usetToDelete;
    }
}