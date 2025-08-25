using Commerce.Application.Orders;
using Commerce.Domain.Abstractions;
using Commerce.Domain.Pricing;
using Commerce.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Commerce.Infrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    // SRP: this class is only responsible for wiring dependencies (composition root)
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration config)
    {
        var conn = config.GetConnectionString("OracleDb");
        services.AddDbContext<CommerceDbContext>(opts =>
        {
            // Oracle provider
            opts.UseOracle(conn);
        });

        services.AddScoped(typeof(IRepository<>), typeof(EfRepository<>));
        // Pricing strategy selection by config (OCP-ready)
        var pricingMode = config.GetValue<string>("Pricing:Mode") ?? "regular";
        if (string.Equals(pricingMode, "discount", StringComparison.OrdinalIgnoreCase))
        {
            var percent = config.GetValue<decimal?>("Pricing:DiscountPercent") ?? 0.10m;
            services.AddSingleton<IPricingStrategy>(new DiscountPricingStrategy(percent));
        }
        else
        {
            services.AddSingleton<IPricingStrategy, RegularPricingStrategy>();
        }

        services.AddScoped<IOrderService, OrderService>();
        return services;
    }
}
