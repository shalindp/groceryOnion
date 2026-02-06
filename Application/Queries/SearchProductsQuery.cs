using Application.Enums;
using Application.Interfaces;
using Application.Models;
using FuzzySharp;
using Persistence;

namespace Application.Queries;

public record SearchProductsQueryRequest(string Term, int Limit, int Skip);

public record SearchProductsQueryResult(IList<ProductDto> Products);

public class SearchProductsQuery : IQuery<Result<SearchProductsQueryResult>, SearchProductsQueryRequest>
{
    private readonly INpgsqlDbContext _dbContext;
    private const int MinimumFuzzyScore = 60; // Minimum score to include a product in results

    public SearchProductsQuery(INpgsqlDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Result<SearchProductsQueryResult>> SendAsync(SearchProductsQueryRequest requestBody)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(requestBody.Term))
            {
                return Result<SearchProductsQueryResult>.Failure("Search term cannot be empty");
            }

            var allProducts = await _dbContext.Queries.GetAllProducts();

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
                    var nameScore = Fuzz.PartialRatio(requestBody.Term, firstProduct.Name);
                    var brandScore = Fuzz.PartialRatio(requestBody.Term, firstProduct.Brand ?? "");
                    
                    // Weight: Name matches are weighted more heavily
                    var weightedScore = (nameScore * 0.7) + (brandScore * 0.3);
                    
                    return new
                    {
                        Product = firstProduct,
                        Stores = group.Select(p => p.StoreName).Distinct().ToList(),
                        Score = (int)weightedScore
                    };
                })
                .Where(x => x.Score >= MinimumFuzzyScore)
                .OrderByDescending(x => x.Score)
                .ThenBy(x => x.Product.Name)
                .Skip(requestBody.Skip)
                .Take(requestBody.Limit)
                .ToList();

            // Build ProductDto with PricingUrls for each store
            var productDtos = searchResults
                .Select(result =>
                {
                    var pricingUrls = result.Stores
                        .Select(storeName => new PricingUrlDto
                        {
                            StoreName = ParseStoreName(storeName),
                            Sku = result.Product.StoreSku
                        })
                        .ToList();

                    return new ProductDto
                    {
                        ProductId = result.Product.ProductId,
                        Barcode = result.Product.Barcode,
                        Name = result.Product.Name,
                        Brand = result.Product.Brand,
                        StoreType = ParseStoreName(result.Stores.First()),
                        ImageUrl = result.Product.ImageUrl,
                        MaxQuantity = (float)(result.Product.MaxQuantity ?? 0),
                        PricingUrls = pricingUrls
                    };
                })
                .ToList();

            return Result<SearchProductsQueryResult>.Success(new SearchProductsQueryResult(productDtos));
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