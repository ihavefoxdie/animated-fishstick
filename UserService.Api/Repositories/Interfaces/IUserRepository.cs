namespace UserService.Api.Repositories.Interfaces;

public interface IUserRepository<T> : IDisposable
{
    Task Create(T user);
    //Task Update(T user);
    Task<IEnumerable<T>> Read();
    //Task<T> Read(T user);
}
