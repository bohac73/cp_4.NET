namespace Commerce.Application.Orders;

public record CreateOrderDto(string CustomerEmail, List<CreateOrderItemDto> Items);
public record CreateOrderItemDto(string Sku, int Quantity, decimal UnitPrice);
