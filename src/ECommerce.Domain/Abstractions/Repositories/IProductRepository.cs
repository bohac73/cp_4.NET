
using ECommerce.Domain.Entities;

namespace ECommerce.Domain.Abstractions.Repositories;

public interface IProductRepository
{
    Task<IEnumerable<Product>> GetAllAsync(CancellationToken ct = default);
    Task<Product?> GetByIdAsync(int id, CancellationToken ct = default);
    Task<Product> AddAsync(Product product, CancellationToken ct = default);
}
