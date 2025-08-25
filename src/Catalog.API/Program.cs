
using ECommerce.Application.Services;
using ECommerce.Infrastructure.Data;
using ECommerce.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Swagger
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Oracle EF Core
builder.Services.AddDbContext<AppDbContext>(options =>
{
    var cs = builder.Configuration.GetConnectionString("OracleDb");
    options.UseOracle(cs ?? "User Id=USER;Password=PWD;Data Source=localhost:1521/XEPDB1");
});

// DI
builder.Services.AddScoped<IProductService, ProductService>();
builder.Services.AddScoped<ECommerce.Domain.Abstractions.Repositories.IProductRepository, ProductRepository>();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.MapGet("/api/products", async (IProductService svc, CancellationToken ct) =>
{
    var items = await svc.ListAsync(ct);
    return Results.Ok(items);
})
.WithName("GetProducts")
.Produces<IEnumerable<ECommerce.Domain.Entities.Product>>(StatusCodes.Status200OK);

app.MapGet("/api/products/{id:int}", async (int id, IProductService svc, CancellationToken ct) =>
{
    var item = await svc.GetAsync(id, ct);
    return item is not null ? Results.Ok(item) : Results.NotFound();
});

app.MapPost("/api/products", async (ECommerce.Domain.Entities.Product p, IProductService svc, CancellationToken ct) =>
{
    var created = await svc.CreateAsync(p, ct);
    return Results.Created($"/api/products/{created.Id}", created);
});

app.Run();
