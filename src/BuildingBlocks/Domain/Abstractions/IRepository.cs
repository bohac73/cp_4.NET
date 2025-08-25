using System.Linq.Expressions;

namespace Commerce.Domain.Abstractions;

public interface IRepository<T> where T : class
{
    Task<T?> GetAsync(int id);
    Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null);
    Task<T> AddAsync(T entity);
    Task UpdateAsync(T entity);
    Task DeleteAsync(T entity);
    Task<int> SaveChangesAsync();
}
