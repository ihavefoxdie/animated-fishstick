namespace UserService.Api.Repositories.Interfaces;

public interface IUserRepository<T> : IDisposable
{
    void Create(T user);
    void Update(T user);
    IEnumerable<T> Read();
    T Read(T user);
}
