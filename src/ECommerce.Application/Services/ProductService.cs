
using ECommerce.Domain.Abstractions.Repositories;
using ECommerce.Domain.Entities;

namespace ECommerce.Application.Services;

// SRP: ProductService only coordinates product use-cases.
public interface IProductService
{
    Task<IEnumerable<Product>> ListAsync(CancellationToken ct = default);
    Task<Product?> GetAsync(int id, CancellationToken ct = default);
    Task<Product> CreateAsync(Product product, CancellationToken ct = default);
}

public class ProductService : IProductService
{
    private readonly IProductRepository _repo;

    // DIP: Depend on interface, not concrete implementation
    public ProductService(IProductRepository repo) => _repo = repo;

    public Task<IEnumerable<Product>> ListAsync(CancellationToken ct = default)
        => _repo.GetAllAsync(ct);

    public Task<Product?> GetAsync(int id, CancellationToken ct = default)
        => _repo.GetByIdAsync(id, ct);

    public Task<Product> CreateAsync(Product product, CancellationToken ct = default)
        => _repo.AddAsync(product, ct);
}
