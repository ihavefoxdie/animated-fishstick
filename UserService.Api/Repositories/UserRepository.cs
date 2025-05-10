using UserService.Api.Data;
using UserService.Api.Repositories.Interfaces;
using UserService.Models;
using UserService.Models.DTOs;

namespace UserService.Api.Repositories;

public class UserRepository : IUserRepository<UserDTO>, IDisposable
{
    private readonly UserDbContext _userDbContext;

    public UserRepository(UserDbContext userDbContext)
    {
        _userDbContext = userDbContext;
    }

    public void Create(UserDTO user)
    {
        throw new NotImplementedException();
    }

    public void Dispose()
    {
        throw new NotImplementedException();
    }

    public IEnumerable<UserDTO> Read()
    {
        throw new NotImplementedException();
    }

    public UserDTO Read(UserDTO user)
    {
        throw new NotImplementedException();
    }

    public void Update(UserDTO user)
    {
        throw new NotImplementedException();
    }
}