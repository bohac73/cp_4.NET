using Commerce.Domain.Abstractions;
using Commerce.Domain.Entities;
using Commerce.Domain.Pricing;

namespace Commerce.Application.Orders;

public interface IOrderService
{
    Task<Order> CreateAsync(CreateOrderDto dto);
    Task<Order?> GetAsync(int id);
}

public sealed class OrderService(IRepository<Order> repo, IPricingStrategy pricing) : IOrderService
{
    private readonly IRepository<Order> _repo = repo;
    private readonly IPricingStrategy _pricing = pricing; // DIP: depends on abstraction

    public async Task<Order> CreateAsync(CreateOrderDto dto)
    {
        var order = new Order
        {
            CustomerEmail = dto.CustomerEmail,
            Items = dto.Items.Select(i => new OrderItem
            {
                Sku = i.Sku,
                Quantity = i.Quantity,
                UnitPrice = i.UnitPrice
            }).ToList()
        };
        var baseTotal = order.Items.Sum(i => i.UnitPrice * i.Quantity);
        order.Total = _pricing.Calculate(baseTotal); // OCP via strategy
        await _repo.AddAsync(order);
        await _repo.SaveChangesAsync();
        return order;
    }

    public Task<Order?> GetAsync(int id) => _repo.GetAsync(id);
}
