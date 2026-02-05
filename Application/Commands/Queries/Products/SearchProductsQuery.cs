using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Interfaces;
using Application.Models;
using Persistence;

namespace Application.Commands.Queries.Products;

public record SearchProductsQueryRequest(Context Context, string SearchTerm, int[] WoolworthAreaIds);
public record SearchProductsQueryResult(IList<StoreProduct> StoreProducts);

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