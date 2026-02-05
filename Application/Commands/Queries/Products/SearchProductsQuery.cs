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

            return null;
        }
        catch (Exception ex)
        {
            return Result<SearchProductsQueryResult>.Failure($"Error searching products: {ex.Message}");
        }
    }
}