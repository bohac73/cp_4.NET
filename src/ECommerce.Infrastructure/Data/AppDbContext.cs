
using ECommerce.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace ECommerce.Infrastructure.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<Product> Products => Set<Product>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        // OCP: Mapping can be extended by new configurations without changing DbContext
        modelBuilder.Entity<Product>(b =>
        {
            b.ToTable("PRODUCTS");
            b.HasKey(p => p.Id);
            b.Property(p => p.Id).ValueGeneratedOnAdd();
            b.Property(p => p.Name).IsRequired().HasMaxLength(200);
            b.Property(p => p.Description).HasMaxLength(2000);
            b.Property(p => p.Price).HasColumnType("NUMBER(18,2)");
        });
    }
}
