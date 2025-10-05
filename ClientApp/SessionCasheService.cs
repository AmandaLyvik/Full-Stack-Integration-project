using Blazored.SessionStorage;

public interface ISessionCacheService
{
    Task<T?> GetAsync<T>(string key) where T : class;
    Task SetAsync<T>(string key, T value, TimeSpan? duration = null) where T : class;
    Task<bool> IsValidAsync(string key, TimeSpan duration);
    Task ClearAsync(string key);
}


public class SessionCacheService : ISessionCacheService
{
    private readonly ISessionStorageService _sessionStorage;

    public SessionCacheService(ISessionStorageService sessionStorage)
    {
        _sessionStorage = sessionStorage;
    }

    private class CacheWrapper<T>
    {
        public T? Data { get; set; }
        public DateTime CachedAt { get; set; }
    }

    public async Task<T?> GetAsync<T>(string key) where T : class
    {
        var wrapper = await _sessionStorage.GetItemAsync<CacheWrapper<T>>(key);
        return wrapper?.Data;
    }

    public async Task SetAsync<T>(string key, T value, TimeSpan? duration = null) where T : class
    {
        var wrapper = new CacheWrapper<T>
        {
            Data = value,
            CachedAt = DateTime.UtcNow
        };

        await _sessionStorage.SetItemAsync(key, wrapper);
    }

    public async Task<bool> IsValidAsync(string key, TimeSpan duration)
    {
        var wrapper = await _sessionStorage.GetItemAsync<CacheWrapper<object>>(key);
        return wrapper != null && DateTime.UtcNow - wrapper.CachedAt < duration;
    }

    public async Task ClearAsync(string key)
    {
        await _sessionStorage.RemoveItemAsync(key);
    }
}
