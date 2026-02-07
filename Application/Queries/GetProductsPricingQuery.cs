using Application.Actions.Products;
using Application.Enums;
using Application.Interfaces;
using Application.Services;
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
    private readonly IWoolworthsThrottleService _woolworthsThrottleService;
    private readonly ICacheService _cacheService;

    public GetProductsPricingQuery(INpgsqlDbContext dbContext, IWoolworthsProductAction woolworthsProductAction, IWoolworthsThrottleService woolworthsThrottleService, ICacheService cacheService)
    {
        _dbContext = dbContext;
        _woolworthsProductAction = woolworthsProductAction;
        _woolworthsThrottleService = woolworthsThrottleService;
        _cacheService = cacheService;
    }

    public record AA(string StoreSku, WoolworthsSession WoolworthsSession);
    public async Task<GetProductsPricingQueryResponse> SendAsync(GetProductsPricingQueryRequest request)
    {
        var woolworthsSessions =
            (await _cacheService.GetCachedWoolworthsSessionAsync(
                request.WoolworthStoreIds,
                () => _dbContext.Queries.getWoolworthsSession(new QueriesSql.getWoolworthsSessionArgs(request.WoolworthStoreIds))))
            .Select(c => c.WoolworthsSession)
            .ToList();

        var woolworthTasks = new List<AA>();

        foreach (var woolworthsStoreId in request.WoolworthStoreIds)
        {
            foreach (var productIdAndStoreSku in request.ProductIdAndStoreSkus)
            {
                var woolworthsSession = woolworthsSessions.First(c => c?.AddressId == woolworthsStoreId)!.Value;
                
                // var woolworthTask = _woolworthsThrottleService.ExecuteAsync(() => _woolworthsProductAction.GetProductPriceAsync(productIdAndStoreSku.StoreSku, woolworthsSession));
                // var woolworthTask = _woolworthsProductAction.GetProductPriceAsync(productIdAndStoreSku.StoreSku, woolworthsSession);
                woolworthTasks.Add(new AA(productIdAndStoreSku.StoreSku, woolworthsSession));
            }
        }

        (string storeSku, WoolworthsSession session)[] tupleArray = 
            woolworthTasks.Select(x => (x.StoreSku, x.WoolworthsSession))
                .ToArray();
        
        var prices = await _woolworthsThrottleService.ExecuteAsync(()=> _woolworthsProductAction.GetProductPricesAsync(tupleArray));
        Console.WriteLine(string.Join(", ", prices));

        return new GetProductsPricingQueryResponse();
    }
}