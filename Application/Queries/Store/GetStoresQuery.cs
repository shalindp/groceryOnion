using Application.Actions.Regions;
using Application.Actions.Session;
using Application.Enums;
using Application.Interfaces;
using Persistence;

namespace Application.Queries.Store;

public record StoreQueryResponse
{
    public string StoreId { get; init; }
    public string StoreRegionName { get; init; }
    public StoreName StoreName { get; init; }
}

public class GetStoresQuery : IQuery<IList<StoreQueryResponse>>
{
    private readonly IWoolworthsStoreAction _woolworthsStoreAction;
    private readonly IPaknSaveStoreAction _paknSaveStoreAction;
    private readonly IPaknSaveSessionAction _paknSaveSessionAction;
    private readonly INpgsqlDbContext _dbContext;

    public GetStoresQuery(IWoolworthsStoreAction woolworthsStoreAction, IPaknSaveStoreAction paknSaveStoreAction,
        IPaknSaveSessionAction paknSaveSessionAction, INpgsqlDbContext dbContext)
    {
        _woolworthsStoreAction = woolworthsStoreAction;
        _paknSaveStoreAction = paknSaveStoreAction;
        _paknSaveSessionAction = paknSaveSessionAction;
        _dbContext = dbContext;
    }

    public async Task<IList<StoreQueryResponse>> SendAsync()
    {
        return await _dbContext.WithTransactionAsync(async sql =>
        {
            var woolworthStores = _woolworthsStoreAction.GetStoresAsync();

            var paknSaveSession = await _paknSaveSessionAction.GetOrCreateSessionAsync(sql);
            var paknSaveStores = _paknSaveStoreAction.GetStoresAsync(paknSaveSession);

            var stores = (await Task.WhenAll(woolworthStores, paknSaveStores)).SelectMany(c => c).ToList();
            return stores;
        });
    }
}