using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Shared.Models;
using System.ComponentModel.DataAnnotations;


public class ProductService
{
    private readonly HttpClient _http;
    private readonly IValidator<Product[]> _validator;
    private readonly IJsonParser<Product[]> _parser;
    private readonly ConsoleLogger _logger;
    private readonly ISessionCacheService _cache;
    private const string CacheKey = "cachedProducts";
    private readonly TimeSpan _cacheDuration = TimeSpan.FromMinutes(5);

    public ProductService(
        HttpClient http,
        IValidator<Product[]> validator,
        IJsonParser<Product[]> parser,
        ConsoleLogger logger,
        ISessionCacheService cache
    )
    {
        _http = http;
        _validator = validator;
        _parser = parser;
        _logger = logger;
        _cache = cache;
    }

    public async Task<(Product[]? products, string? error)> GetProductsAsync()
    {
        // Check if the data is in cache and still valid
        if (await _cache.IsValidAsync(CacheKey, _cacheDuration))
        {
            var cached = await _cache.GetAsync<Product[]>(CacheKey);
            if (cached is { Length: > 0 })
            {
                return (cached, null);
            }
        }

        // Not in cache or cache expired, fetch from server
        try
        {
            var response = await _http.GetAsync("/api/productlist");

            if (!response.IsSuccessStatusCode)
            {
                return (null, $"Server error: {(int)response.StatusCode} {response.ReasonPhrase}");
            }

            var json = await response.Content.ReadAsStringAsync();

            try
            {
                var products = _parser.Parse(json);

                if (!_validator.IsValid(products, out var validationErrors))
                {
                    foreach (var error in validationErrors)
                    {
                        await _logger.ErrorAsync($"Validation error: {error.ErrorMessage}");
                    }

                    return (null, "Product data failed validation.");
                }

                await _cache.SetAsync(CacheKey, products);
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
