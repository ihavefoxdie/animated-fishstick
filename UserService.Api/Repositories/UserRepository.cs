using UserService.Api.Data;
using UserService.Api.Repositories.Interfaces;
using UserService.Api.Entities;
using Microsoft.EntityFrameworkCore;

namespace UserService.Api.Repositories;

public class UserRepository : IUserRepository<User>, IDisposable
{
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

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public async Task<IEnumerable<User>> Read()
    {
        return await _userDbContext.Users.ToListAsync();
    }

    public async Task<User> Read(User user)
    {
        throw new NotImplementedException();
    }

    public async Task Update(User user)
    {
        throw new NotImplementedException();
    }
}