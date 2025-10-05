using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using Shared.Models;
using System.ComponentModel.DataAnnotations;

public static class ValidationHelper
{
    public static bool IsValid(object obj, out List<ValidationResult> results)
    {
        results = new List<ValidationResult>();
        var context = new ValidationContext(obj);
        bool isValid = Validator.TryValidateObject(obj, context, results, true);

        foreach (var property in obj.GetType().GetProperties())
        {
            var value = property.GetValue(obj);
            if (value == null || property.PropertyType.IsPrimitive || property.PropertyType == typeof(string))
                continue;

            if (value is IEnumerable<object> collection)
            {
                foreach (var item in collection)
                {
                    if (!IsValid(item, out var nestedResults))
                    {
                        isValid = false;
                        results.AddRange(nestedResults);
                    }
                }
            }
            else
            {
                if (!IsValid(value, out var nestedResults))
                {
                    isValid = false;
                    results.AddRange(nestedResults);
                }
            }
        }

        return isValid;
    }
}

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

                if (!ValidationHelper.IsValid(products, out var validationErrors))
                {
                    foreach (var error in validationErrors)
                    {
                        await _logger.ErrorAsync($"Validation error: {error.ErrorMessage}");
                    }

                    return (null, "Product data failed validation.");
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
