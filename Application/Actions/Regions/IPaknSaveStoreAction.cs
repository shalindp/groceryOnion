using Application.Enums;
using Application.Helpers;
using Application.Queries.Store;
using Persistence;

namespace Application.Actions.Regions;

public interface IPaknSaveStoreAction
{
    public Task<IList<StoreQueryResponse>> GetStoresAsync(PaknsaveSession session);
}

class PaknSaveStoreAction : IPaknSaveStoreAction
{
    private readonly IHttpHelper _httpHelper;

    public PaknSaveStoreAction(IHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    private record StoresResponse(StoreResponse[] Stores);

    private record StoreResponse(string Id, string Name);

    public async Task<IList<StoreQueryResponse>> GetStoresAsync(PaknsaveSession session)
    {
        var headers = PaknSaveHelper.BuildAuthenticationHeader(session.AccessToken);
        const string url = "https://api-prod.paknsave.co.nz/v1/edge/store";
        var response = await _httpHelper.GetAsync<StoresResponse>(url, headers);
        return response.Body.Stores.Select(c => new StoreQueryResponse
        {
            StoreId = c.Id,
            StoreRegionName = c.Name,
            StoreName = StoreName.PaknSave
        }).ToList();
    }
    

}