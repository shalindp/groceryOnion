using Application.Actions.Products;
using Application.Actions.Session;
using Application.Enums;
using Application.Interfaces;
using Application.Services;
using Persistence;

namespace Application.Queries.Product;

public class ProductPriceQueryRequest
{
    public Guid ProductId { get; set; }
    public StoreName StoreName { get; init; }
    public string StoreId { get; init; }
    public string RegionStoreName { get; set; }
    public string StoreSku { get; init; }
    public double Price { get; set; } = 0.0;
    public IList<ProductMultiBuy> MultiBuys = new List<ProductMultiBuy>();
}

public class ProductMultiBuy
{
    public double PriceWhenQuantityIsMet { get; init; } = 0;
    public double QuantityRequired { get; init; } = 0;
}

public class GetProductsPricingQuery : IQuery<ProductPriceQueryRequest[], ProductPriceQueryRequest[]>
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IWoolworthsThrottleService _woolworthsThrottleService;
    private readonly IPaknSaveProductAction _paknSaveProductAction;
    private readonly ICacheService _cacheService;
    private readonly IPaknSaveSessionAction _paknSaveSessionAction;
    private readonly IWoolworthsSessionAction _woolworthsSessionAction;
    public GetProductsPricingQuery(INpgsqlDbContext dbContext, IWoolworthsProductAction woolworthsProductAction, IWoolworthsThrottleService woolworthsThrottleService,
        ICacheService cacheService, IPaknSaveProductAction paknSaveProductAction, IPaknSaveSessionAction paknSaveSessionAction, IWoolworthsSessionAction woolworthsSessionAction)
    {
        _dbContext = dbContext;
        _woolworthsProductAction = woolworthsProductAction;
        _woolworthsThrottleService = woolworthsThrottleService;
        _cacheService = cacheService;
        _paknSaveProductAction = paknSaveProductAction;
        _paknSaveSessionAction = paknSaveSessionAction;
        _woolworthsSessionAction = woolworthsSessionAction;
    }


    public async Task<ProductPriceQueryRequest[]> SendAsync(ProductPriceQueryRequest[] request)
    {
        return await _dbContext.WithTransactionAsync<ProductPriceQueryRequest[]>(async (dbContext) =>
        {
            #region woolworths

            var woolworthsRequests = request.Where(c => c.StoreName == StoreName.Woolworths)
                .ToArray();

            var woolworthsStoreIds = woolworthsRequests.Select(c => c.StoreId).ToArray();
            var woolworthsSessions = await _woolworthsSessionAction.GetOrCreateSessionAsync(dbContext, woolworthsStoreIds);

            var woolworthTasks = new List<WoolworthsStoreSkuAndSessionActionArg>();
            foreach (var storePriceHolder in woolworthsRequests)
            {
                var session = woolworthsSessions.First(c => c.Value.StoreId.Equals(storePriceHolder.StoreId));
                woolworthTasks.Add(new WoolworthsStoreSkuAndSessionActionArg(storePriceHolder, session.Value));
            }

            #endregion

            #region paknsave

            var paknSaveRequests = request.Where(c => c.StoreName == StoreName.PaknSave)
                .ToArray();

            var pakSaveSession = await _paknSaveSessionAction.GetOrCreateSessionAsync(dbContext);

            var paknSaveTasks = new List<Task<ProductPriceQueryRequest>>();
            foreach (var storePriceHolder in paknSaveRequests)
            {
                var task = _paknSaveProductAction.GetProductPricingAsync(storePriceHolder, pakSaveSession.AccessToken);
                paknSaveTasks.Add(task);
            }

            var woolworthsPrices = _woolworthsThrottleService.ExecuteAsync(() => _woolworthsProductAction.GetProductPricesAsync(woolworthTasks));
            var paknSavePrices = Task.WhenAll(paknSaveTasks);

            #endregion

            var prices = (await Task.WhenAll(woolworthsPrices, paknSavePrices)).SelectMany(c => c).ToArray();

            return prices;
        });
    }
}