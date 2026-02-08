using Application.Actions.Regions;
using Application.Actions.Session;
using Application.Enums;
using Application.Interfaces;

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

    public GetStoresQuery(IWoolworthsStoreAction woolworthsStoreAction, IPaknSaveStoreAction paknSaveStoreAction,
        IPaknSaveSessionAction paknSaveSessionAction)
    {
        _woolworthsStoreAction = woolworthsStoreAction;
        _paknSaveStoreAction = paknSaveStoreAction;
        _paknSaveSessionAction = paknSaveSessionAction;
    }

    public async Task<IList<StoreQueryResponse>> SendAsync()
    {
        var woolworthStores = _woolworthsStoreAction.GetStoresAsync();

        var paknSaveSession = await _paknSaveSessionAction.GetOrCreateSessionAsync();
        var paknSaveStores = _paknSaveStoreAction.GetStoresAsync(paknSaveSession);

        var stores = (await Task.WhenAll(woolworthStores, paknSaveStores)).SelectMany(c => c).ToList();
        return stores;
    }
}