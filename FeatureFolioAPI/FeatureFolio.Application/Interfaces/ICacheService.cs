namespace FeatureFolio.Application.Interfaces;

public interface ICacheService
{
    public Task SetAsync<T>(string key, T data);
    public Task<T?> GetAsync<T>(string key);
    public Task RemoveAsync(string key);
}
