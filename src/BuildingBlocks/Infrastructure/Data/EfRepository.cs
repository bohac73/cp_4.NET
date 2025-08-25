using Commerce.Domain.Abstractions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Commerce.Infrastructure.Data;

public class EfRepository<T>(CommerceDbContext db) : IRepository<T> where T : class
{
    private readonly CommerceDbContext _db = db;

    public async Task<T> AddAsync(T entity)
    {
        await _db.Set<T>().AddAsync(entity);
        return entity;
    }

    public Task DeleteAsync(T entity)
    {
        _db.Set<T>().Remove(entity);
        return Task.CompletedTask;
    }

    public Task<T?> GetAsync(int id) => _db.Set<T>().FindAsync(id).AsTask();

    public async Task<IReadOnlyList<T>> ListAsync(Expression<Func<T, bool>>? predicate = null)
    {
        IQueryable<T> q = _db.Set<T>();
        if (predicate != null) q = q.Where(predicate);
        return await q.ToListAsync();
    }

    public Task UpdateAsync(T entity)
    {
        _db.Update(entity);
        return Task.CompletedTask;
    }

    public Task<int> SaveChangesAsync() => _db.SaveChangesAsync();
}
