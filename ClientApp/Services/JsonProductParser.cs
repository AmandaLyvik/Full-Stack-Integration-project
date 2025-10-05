using Shared.Models;
using System.Text.Json;

public interface IJsonParser<T>
{
    T? Parse(string json);
}

public class JsonProductParser : IJsonParser<Product[]>
{
    public Product[]? Parse(string json)
    {
        var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
        return JsonSerializer.Deserialize<Product[]>(json, options);
    }
}
