namespace Commerce.Domain.Pricing;

public interface IPricingStrategy
{
    decimal Calculate(decimal baseTotal);
}

public sealed class RegularPricingStrategy : IPricingStrategy
{
    public decimal Calculate(decimal baseTotal) => baseTotal;
}

public sealed class DiscountPricingStrategy : IPricingStrategy
{
    private readonly decimal _percent;
    public DiscountPricingStrategy(decimal percent) => _percent = percent;
    public decimal Calculate(decimal baseTotal) => baseTotal * (1m - _percent);
}
