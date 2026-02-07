﻿using Application.Models;
using Persistence;

namespace Application.Services;

public interface ICacheService
{
    public Task<List<QueriesSql.GetAllProductsRow>> GetCachedAllProductsWithCacheAsync(
        Func<Task<List<QueriesSql.GetAllProductsRow>>> fetchFunction);

    public Task<List<QueriesSql.getWoolworthsSessionRow>> GetCachedWoolworthsSessionAsync(
        int[] storeIds,
        Func<Task<List<QueriesSql.getWoolworthsSessionRow>>> fetchFunction);
}

class CacheService : ICacheService
{
    private record CachedWithExpiry<T>(T Value, DateTime Expiry);

    private CachedWithExpiry<List<QueriesSql.GetAllProductsRow>> _cachedAllProducts = new([], DateTime.UtcNow);
    
    private Dictionary<Guid, CachedWithExpiry<ProductPricingDto>> _productPricingCache = new();

    private Dictionary<string, CachedWithExpiry<List<QueriesSql.getWoolworthsSessionRow>>> _woolworthsSessionCache = new();

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

    public async Task<List<QueriesSql.getWoolworthsSessionRow>> GetCachedWoolworthsSessionAsync(
        int[] storeIds,
        Func<Task<List<QueriesSql.getWoolworthsSessionRow>>> fetchFunction)
    {
        var cacheKey = string.Join("-", storeIds.OrderBy(x => x));

        if (_woolworthsSessionCache.TryGetValue(cacheKey, out var cached) && cached.Expiry > DateTime.UtcNow)
        {
            return cached.Value;
        }

        var newValue = await fetchFunction();
        _woolworthsSessionCache[cacheKey] = new CachedWithExpiry<List<QueriesSql.getWoolworthsSessionRow>>(newValue, DateTime.UtcNow.AddMinutes(15));
        return newValue;
    }
}