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

    public GetProductsPricingQuery(INpgsqlDbContext dbContext, IWoolworthsProductAction woolworthsProductAction, IWoolworthsThrottleService woolworthsThrottleService,
        ICacheService cacheService)
    {
        _dbContext = dbContext;
        _woolworthsProductAction = woolworthsProductAction;
        _woolworthsThrottleService = woolworthsThrottleService;
        _cacheService = cacheService;
    }

    public async Task<GetProductsPricingQueryResponse> SendAsync(GetProductsPricingQueryRequest request)
    {
        var woolworthsSessions = (await _dbContext.Queries.getWoolworthsSession(new QueriesSql.getWoolworthsSessionArgs(request.WoolworthStoreIds)))
            .Select(c => c.WoolworthsSession)
            .ToList();

        var woolworthTasks = new List<WoolworthsStoreSkuAndSessionArg>();

        foreach (var woolworthsStoreId in request.WoolworthStoreIds)
        {
            foreach (var productIdAndStoreSku in request.ProductIdAndStoreSkus)
            {
                var woolworthsSession = woolworthsSessions.First(c => c?.AddressId == woolworthsStoreId)!.Value;
                woolworthTasks.Add(new WoolworthsStoreSkuAndSessionArg(productIdAndStoreSku.StoreSku, woolworthsSession));
            }
        }

        var prices = await _woolworthsThrottleService.ExecuteAsync(() => _woolworthsProductAction.GetProductPricesAsync(woolworthTasks));
        Console.WriteLine(string.Join(", ", prices));

        return new GetProductsPricingQueryResponse();
    }
}