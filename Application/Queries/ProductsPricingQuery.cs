using Application.Actions.Products;
using Application.Enums;
using Application.Interfaces;
using Application.Services;
using Persistence;

namespace Application.Queries;

public class ProductPriceQueryRequest
{
    public Guid ProductId { get; set; }
    public StoreName StoreName { get; init; }
    public string StoreId { get; init; }
    public string StoreSku { get; init; }
    public double Price { get; set; } = 0.0;
}

public class ProductsPricingQuery : IQuery<ProductPriceQueryRequest[], ProductPriceQueryRequest[]>
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IWoolworthsThrottleService _woolworthsThrottleService;
    private readonly IPaknSaveProductAction _paknSaveProductAction;
    private readonly ICacheService _cacheService;

    public ProductsPricingQuery(INpgsqlDbContext dbContext, IWoolworthsProductAction woolworthsProductAction, IWoolworthsThrottleService woolworthsThrottleService,
        ICacheService cacheService, IPaknSaveProductAction paknSaveProductAction)
    {
        _dbContext = dbContext;
        _woolworthsProductAction = woolworthsProductAction;
        _woolworthsThrottleService = woolworthsThrottleService;
        _cacheService = cacheService;
        _paknSaveProductAction = paknSaveProductAction;
    }

    
    public async Task<ProductPriceQueryRequest[]> SendAsync(ProductPriceQueryRequest[] request)
    {
        var woolworthsRequests = request.Where(c => c.StoreName == StoreName.Woolworths)
            .ToArray();

        var woolworthsStoreIds = woolworthsRequests.Select(c => int.Parse(c.StoreId)).ToArray();
        var woolworthsSessions = (await _dbContext.Queries.getWoolworthsSession(new QueriesSql.getWoolworthsSessionArgs(woolworthsStoreIds)))
            .Select(c => c.WoolworthsSession)
            .ToList();

        var woolworthTasks = new List<WoolworthsStoreSkuAndSessionArg>();
        foreach (var storePriceHolder in woolworthsRequests)
        {
            var session = woolworthsSessions.First(c => c.Value.AddressId == int.Parse(storePriceHolder.StoreId));
            woolworthTasks.Add(new WoolworthsStoreSkuAndSessionArg(storePriceHolder, session.Value));
            // var woolworthsTask = _woolworthsProductAction.GetProductPriceAsync(storePriceHolder, session!.Value);
            // woolworthTasks.Add(woolworthsTask);
        }
        

        var paknSaveRequests = request.Where(c => c.StoreName == StoreName.PaknSave)
            .ToArray();
        
        var pakSaveSession = (await _dbContext.Queries.getPaknSaveSession())?.PaknsaveSession;

        var paknSaveTasks = new List<Task<ProductPriceQueryRequest>>();
        foreach (var storePriceHolder in paknSaveRequests)
        {
            var task = _paknSaveProductAction.GetProductPricingAsync(storePriceHolder, pakSaveSession.Value.AccessToken);
            paknSaveTasks.Add(task);
        }

        // var x = async () => await Task.WhenAll(woolworthTasks);
        
        var woolworthsPrices = await _woolworthsThrottleService.ExecuteAsync(()=> _woolworthsProductAction.GetProductPricesAsync(woolworthTasks));
        var paknSavePrices = await Task.WhenAll(paknSaveTasks);


        var prices= woolworthsPrices.Concat(paknSavePrices).ToArray();
        return prices;
    }
}