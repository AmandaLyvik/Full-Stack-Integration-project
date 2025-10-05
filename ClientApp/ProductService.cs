using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Shared.Models;

public class ProductService
{
    private readonly HttpClient _http;
    private readonly ConsoleLogger _logger;

    public ProductService(HttpClient http, ConsoleLogger logger)
    {
        _http = http;
        _logger = logger;
    }

    public async Task<(Product[]? products, string? error)> GetProductsAsync()
    {
        try
        {
            var response = await _http.GetAsync("/api/productlist");

            if (!response.IsSuccessStatusCode)
            {
                return (null, $"Server error: {(int)response.StatusCode} {response.ReasonPhrase}");
            }

            var json = await response.Content.ReadAsStringAsync();

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            };

            try
            {
                var products = JsonSerializer.Deserialize<Product[]>(json, options);

                if (products == null || products.Length == 0)
                {
                    return (null, "No products found or invalid data structure.");
                }

                return (products, null);
            }
            catch (JsonException ex)
            {
                await _logger.ErrorAsync($"JSON deserialization error: {ex.Message}");
                return (null, $"Invalid JSON format: {ex.Message}");
            }
        }
        catch (HttpRequestException ex)
        {
            await _logger.ErrorAsync($"Network error: {ex.Message}");
            return (null, $"Network error: {ex.Message}");
        }
        catch (TaskCanceledException)
        {
            return (null, "Request timed out.");
        }
        catch (Exception ex)
        {
            return (null, $"Unexpected error: {ex.Message}");
        }
    }
}
