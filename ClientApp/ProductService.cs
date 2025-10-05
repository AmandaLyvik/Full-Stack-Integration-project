using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Shared.Models;


public class ProductService
{
    private readonly HttpClient _http;

    public ProductService(HttpClient http)
    {
        _http = http;
    }

    public async Task<(Product[]? products, string? error)> GetProductsAsync()
    {
        try
        {
            var response = await _http.GetAsync("/api/products");

            if (response.IsSuccessStatusCode)
            {
                var products = await response.Content.ReadFromJsonAsync<Product[]>();
                return (products, null);
            }
            else
            {
                return (null, $"Server error: {(int)response.StatusCode} {response.ReasonPhrase}");
            }
        }
        catch (HttpRequestException ex)
        {
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
