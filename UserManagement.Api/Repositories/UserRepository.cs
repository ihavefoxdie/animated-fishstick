using UserManagement.Api.Data;
using UserManagement.Api.Repositories.Interfaces;
using UserManagement.Api.Entities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace UserManagement.Api.Repositories;

public class UserRepository : IUserRepository<User>, IDisposable
{
    private bool disposed = false;
    private readonly UserDbContext _userDbContext;

    public UserRepository(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    public async Task Create(User user)
    {
        await _userDbContext.AddAsync(user);
        await _userDbContext.SaveChangesAsync();
    }

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

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    public IEnumerable<User> ReadAll(Func<User, bool> exp)
    {
        return _userDbContext.Users.Where(exp);
    }

    public async Task<User?> Read(Expression<Func<User, bool>> exp)
    {
        return await _userDbContext.Users.FirstOrDefaultAsync(exp, default);
    }

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
            userToUpdate.RevokedOn = DateTime.UtcNow;
            userToUpdate.RevokedBy = user.RevokedBy;
            await _userDbContext.SaveChangesAsync();
        }

        return userToUpdate;
    }

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