using Microsoft.AspNetCore.Mvc;
using Shared.Models;

[ApiController]
[Route("api/")]
public class ProductController : ControllerBase
{
    private readonly CacheService _cache;

    public ProductController(CacheService cache)
    {
        _cache = cache;
    }

    // malformed product data for testing
    private readonly Product[] _badProducts = new[]
    {
        new Product
        {
            Id = 0,
            Name = "Invalid product",
            Price = -99.99, // Invalid: Negative price
            Stock = -5, // Invalid: Negative stock
            Category = new Category { Id = 0, Name = "" } // Invalid: Empty name and ID
        },
        new Product
        {
            Id = 1,
            Name = "Missing Category Fields",
            Price = 50.00,
            Stock = 5,
            Category = new Category { Name = "Accessories" } // Invalid: Missing Category.Id
        }
    };

    // good product data
    private readonly Product[] _goodProducts = new[]
    {
        new Product
        {
            Id = 1,
            Name = "Laptop",
            Price = 1200.50,
            Stock = 25,
            Category = new Category { Id = 101, Name = "Electronics" }
        },
        new Product
        {
            Id = 2,
            Name = "Headphones",
            Price = 50.00,
            Stock = 100,
            Category = new Category { Id = 102, Name = "Accessories" }
        }
    };

    [HttpGet("productlist")]
    public ActionResult<IEnumerable<Product>> GetProducts()
    {
        var products = _cache.GetOrCreate("products", entry =>
        {
            entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5);

            return _goodProducts;
        });

        return Ok(products);
    }
}


