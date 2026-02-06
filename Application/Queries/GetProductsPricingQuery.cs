using Application.Actions.Products;
using Application.Enums;
using Application.Interfaces;
using Persistence;

namespace Application.Queries;

public record GetProductsPricingQueryRequest
{
    public int[] WoolworthStoreIds { get; init; }

    public string[] PaknSaveStoreIds { get; init; }

    public ProductIdAndStoreSku[] ProductIdAndStoreSkus { get; init; }
}

public record ProductIdAndStoreSku(Guid ProductId, string StoreSku);

public record StorePrice(Guid ProductId, StoreName StoreName, string StoreId, double Price);

public record GetProductsPricingQueryResponse
{
    public List<StorePrice> StorePrices { get; init; } = [];
}

public class GetProductsPricingQuery : IQuery<GetProductsPricingQueryResponse, GetProductsPricingQueryRequest>
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly IWoolworthsProductAction _woolworthsProductAction;

    public GetProductsPricingQuery(INpgsqlDbContext dbContext, IWoolworthsProductAction woolworthsProductAction)
    {
        _dbContext = dbContext;
        _woolworthsProductAction = woolworthsProductAction;
    }

    public async Task<GetProductsPricingQueryResponse> SendAsync(GetProductsPricingQueryRequest request)
    {
        var woolworthsSessions =
            (await _dbContext.Queries.getWoolworthsSession(new QueriesSql.getWoolworthsSessionArgs(request.WoolworthStoreIds))).Select(c => c.WoolworthsSession)
            .ToList();

        var woolworthTasks = new List<Task<double>>();

        foreach (var woolworthsStoreId in request.WoolworthStoreIds)
        {
            foreach (var productIdAndStoreSku in request.ProductIdAndStoreSkus)
            {
                var woolworthsSession = woolworthsSessions.First(c => c?.AddressId == woolworthsStoreId)!.Value;
                var woolworthTask = _woolworthsProductAction.GetProductPriceAsync(productIdAndStoreSku.StoreSku, woolworthsSession);
                woolworthTasks.Add(woolworthTask);
            }
        }

        var x = await Task.WhenAll(woolworthTasks);

        return new GetProductsPricingQueryResponse();
    }
}