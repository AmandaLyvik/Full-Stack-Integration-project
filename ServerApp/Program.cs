using Shared.Models;

var builder = WebApplication.CreateBuilder(args);

// Add CORS services
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowClient", policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
    options.AddPolicy("AllowUnreachableClient", policy =>
    {
        policy.WithOrigins("http://localhost:9999") 
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

var app = builder.Build();

// Use CORS
app.UseCors("AllowClient");

//app.MapGet("/api/productlist", () =>
//{
//    return new[]
//    {
//        new
//        {
//            Id = 1,
//            Name = "Laptop",
//            Price = 1200.50,
//            Stock = 25,
//            Category = new { Id = 101, Name = "Electronics" }
//        },
//        new
//        {
//            Id = 2,
//            Name = "Headphones",
//            Price = 50.00,
//            Stock = 100,
//            Category = new { Id = 102, Name = "Accessories" }
//        }
//    };
//});

// Endpoint that returns invalid JSON for testing
//app.MapGet("/api/productlist", () =>
//{
//    return Results.Text("This is not valid JSON", "application/json");
//});

// Endpoint that returns malformed product data for testing
app.MapGet("/api/productlist", () =>
{
    return new Product[]
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
});


app.Run();
