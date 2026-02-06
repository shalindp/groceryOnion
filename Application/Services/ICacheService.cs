using Application.Models;
using Persistence;

namespace Application.Services;

public interface ICacheService
{
    public Task<List<QueriesSql.GetAllProductsRow>> GetCachedAllProductsWithCacheAsync(
        Func<Task<List<QueriesSql.GetAllProductsRow>>> fetchFunction);
}

class CacheService : ICacheService
{
    private record CachedWithExpiry<T>(T Value, DateTime Expiry);

    private CachedWithExpiry<List<QueriesSql.GetAllProductsRow>> _cachedAllProducts = new([], DateTime.UtcNow);
    
    private Dictionary<Guid, CachedWithExpiry<ProductPricingDto>> _productPricingCache = new();

    public async Task<List<QueriesSql.GetAllProductsRow>> GetCachedAllProductsWithCacheAsync(
        Func<Task<List<QueriesSql.GetAllProductsRow>>> fetchFunction)
    {
        if (_cachedAllProducts.Expiry < DateTime.UtcNow)
        {
            var newValue = await fetchFunction();
            _cachedAllProducts = new CachedWithExpiry<List<QueriesSql.GetAllProductsRow>>(newValue, DateTime.UtcNow.AddMinutes(15));
            return newValue;
        }

        return _cachedAllProducts.Value;
    }
    
    
}