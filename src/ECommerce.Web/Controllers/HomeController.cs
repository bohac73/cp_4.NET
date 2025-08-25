
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace ECommerce.Web.Controllers;

public class HomeController : Controller
{
    private readonly IHttpClientFactory _cf;

    public HomeController(IHttpClientFactory cf) => _cf = cf;

    public async Task<IActionResult> Index(CancellationToken ct)
    {
        var client = _cf.CreateClient("catalog");
        var resp = await client.GetAsync("api/products", ct);
        var json = await resp.Content.ReadAsStringAsync(ct);
        var products = JsonSerializer.Deserialize<List<ProductVm>>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true }) ?? new List<ProductVm>();
        return View(products);
    }
}

public record ProductVm(int Id, string Name, string? Description, decimal Price);
