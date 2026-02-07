using Application.Enums;
using Application.Interfaces;
using Application.Models;
using Application.Services;
using FuzzySharp;
using Persistence;

namespace Application.Queries;

public record SearchProductsQueryRequest
{
    public string Term { get; init; }
    public int Limit { get; init; }
    public int Skip { get; init; }
}

public record SearchProductsQueryResult
{
    public IList<ProductDto> Products { get; init; }
}

public class SearchProductsQuery : IQuery<Result<SearchProductsQueryResult>, SearchProductsQueryRequest>
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly ICacheService _cacheService;
    private const int MinimumFuzzyScore = 60; // Minimum score to include a product in results

    public SearchProductsQuery(INpgsqlDbContext dbContext, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _cacheService = cacheService;
    }

    public async Task<Result<SearchProductsQueryResult>> SendAsync(SearchProductsQueryRequest request)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(request.Term))
            {
                return Result<SearchProductsQueryResult>.Failure("Search term cannot be empty");
            }

            var allProducts = await _cacheService.GetCachedAllProductsWithCacheAsync(_dbContext.Queries.GetAllProducts);

            // Group products by ProductId to consolidate store information
            var productGroups = allProducts
                .GroupBy(p => p.ProductId)
                .ToList();

            // Perform fuzzy matching with weighted scoring
            var searchResults = productGroups
                .Select(group =>
                {
                    var firstProduct = group.First();

                    // Calculate fuzzy scores for name and brand
                    var nameScore = Fuzz.PartialRatio(request.Term, firstProduct.Name);
                    var brandScore = Fuzz.PartialRatio(request.Term, firstProduct.Brand ?? "");

                    // Weight: Name matches are weighted more heavily
                    var weightedScore = (nameScore * 0.7) + (brandScore * 0.3);

                    return new
                    {
                        Product = firstProduct,
                        Stores = group.Select(p => new {p.StoreName, p.StoreSku}).Distinct().ToList(),
                        Score = (int)weightedScore
                    };
                })
                .Where(x => x.Score >= MinimumFuzzyScore)
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Product.Name)
                .Skip(request.Skip)
                .Take(request.Limit)
                .ToList();

            // Build ProductDto with PricingUrls for each store
            var productDtos = searchResults
                .Select(result =>
                {
                    return new ProductDto
                    {
                        ProductId = result.Product.ProductId,
                        Barcode = result.Product.Barcode,
                        StoreSkus = result.Stores.Select(c=> new StoreSkuDto
                        {
                            StoreName = c.StoreName.ToStoreNameEnum(),
                            StoreSkus = c.StoreSku,
                        }).ToList(),
                        // StoreSku = result.Product.StoreSku,
                        Name = result.Product.Name,
                        Brand = result.Product.Brand,
                        // StoreType = ParseStoreName(result.Stores.First()),
                        ImageUrl = result.Product.ImageUrl,
                        MaxQuantity = (float)(result.Product.MaxQuantity ?? 0),
                    };
                })
                .ToList();

            return Result<SearchProductsQueryResult>.Success(new SearchProductsQueryResult { Products = productDtos });
        }
        catch (Exception ex)
        {
            return Result<SearchProductsQueryResult>.Failure($"Error searching products: {ex.Message}");
        }
    }

    /// <summary>
    /// Parses a store name string from the database to the StoreName enum
    /// </summary>
    private StoreName ParseStoreName(string storeName)
    {
        return storeName.ToLower() switch
        {
            "woolworths" => StoreName.Woolworths,
            "paknsave" => StoreName.PaknSave,
            "newworld" => StoreName.NewWorld,
            _ => throw new InvalidOperationException($"Unknown store name: {storeName}")
        };
    }
}