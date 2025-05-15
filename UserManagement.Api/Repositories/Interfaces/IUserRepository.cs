using System.Linq.Expressions;

namespace UserManagement.Api.Repositories.Interfaces;

public interface IUserRepository<T> : IDisposable
{
    Task Create(T user);
    Task<T?> Update(T user);
    IEnumerable<T> ReadAll(Func<T, bool> exp);
    Task<T?> Read(Expression<Func<T, bool>> exp);
    Task<T?> Delete(T user);
}
