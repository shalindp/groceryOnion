using Application.Actions;
using Application.Interfaces;
using Application.Models;
using Persistence;

namespace Application.Commands.Products;

public record SearchProductsQueryRequest(Context Context, string SearchTerm, int[] WoolworthAreaIds);
public record SearchProductsQueryResult(IList<Product> Products);

public class SearchProductsQuery: IQuery<Result<SearchProductsQueryResult>, SearchProductsQueryRequest>
{
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IWoolworthsRegionAction _woolworthsRegionAction;

    public SearchProductsQuery(IWoolworthsProductAction woolworthsProductAction, IWoolworthsRegionAction woolworthsRegionAction)
    {
        _woolworthsProductAction = woolworthsProductAction;
        _woolworthsRegionAction = woolworthsRegionAction;
    }

    public async Task<Result<SearchProductsQueryResult>> SendAsync(SearchProductsQueryRequest requestBody)
    {
        return null;
    }
}