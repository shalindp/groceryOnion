using Application.Enums;
using Application.Interfaces;
using Application.Models;
using FuzzySharp;
using Persistence;

namespace Application.Commands.Queries.Products;

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
            var products = (await _dbContext.Queries.GetAllProducts())
                .ToDictionary(c => new
                {
                    product = new ProductDto
                    {
                        ProductId = c.ProductId,
                        Barcode = c.Barcode,
                        Name = c.Name,
                        Brand = c.Brand,
                        ImageUrl = c.ImageUrl,
                    },
                    c.StoreName
                });

            //
            // var y = x.GroupBy(c => new { c.Barcode,});
            //
            // // Get all canonical products
            // var allProducts = (await _dbContext.Queries.GetProductWithStoreProducts()).Select(c => c.Product!.Value)
            //     .ToList();
            //
            // // Apply fuzzy search on name and brand
            var searchResults = products
                .Select(product => new
                {
                    Product = product,
                    NameScore = Fuzz.PartialTokenSortRatio(requestBody.Term.ToLower(),
                        product.Key.product.Name.ToLower()),
                    BrandScore = Fuzz.PartialRatio(requestBody.Term.ToLower(),
                        (product.Key.product.Brand ?? "").ToLower())
                })
                .Where(x => x.NameScore >= 80 || x.BrandScore >= MinimumFuzzyScore)
                .OrderByDescending(x => Math.Max(x.NameScore, x.BrandScore)) // Sort by highest fuzzy score
                .ThenBy(x => x.Product.Key.product.ProductId) // Deterministic secondary sort by ID
                .Skip(requestBody.Skip)
                .Take(requestBody.Limit)
                .Select(x => x.Product)
                .ToList();
            //
            return Result<SearchProductsQueryResult>.Success(
                new SearchProductsQueryResult(searchResults.Select(c => new ProductDto
                {
                    Barcode = c.Key.product.Barcode,
                    Name = c.Key.product.Name,
                    Brand = c.Key.product.Brand,
                    StoreType = c.Key.StoreName.ToStoreNameEnum(),
                    MaxQuantity = 32,
                    ImageUrl = c.Key.product.ImageUrl,
                    PricingUrls = new List<PricingUrlDto>()
                    // PricingUrls = c.PricingUrls.Select(url => new PricingUrlDto
                    // {
                        // StoreType = url.StoreType,
                        // Url = url.Url
                    // }).ToList()
                }).ToList())
            );
            // return null;
        }
        catch (Exception ex)
        {
            return Result<SearchProductsQueryResult>.Failure($"Error searching products: {ex.Message}");
        }
    }
}