using Application.Constants;
using Application.Enums;
using Application.Models;
using Application.Queries.Store;

namespace Application.Actions.Regions;

public interface IWoolworthsStoreAction
{
    public Task<CreateSessionWithRegionDto[]> CreateSessionWithRegionsAsync(string[] storeId);
    public Task<CreateSessionWithRegionDto> CreateSessionWithRegionAsync(string storeId);
    public Task<IList<StoreQueryResponse>> GetStoresAsync();
}

public record WoolworthsGetRegionsResult(string StoreId, string StoreName);

public class WoolworthsStoreAction : IWoolworthsStoreAction
{
    private readonly IHttpHelper _httpHelper;

    public WoolworthsStoreAction(IHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    public async Task<IList<StoreQueryResponse>> GetStoresAsync()
    {
        const string url = "https://www.woolworths.co.nz/api/v1/addresses/pickup-addresses";
        var result = await _httpHelper.GetAsync<RegionsResponse?>(url, headers: Headers.WoolworthsDefaultHeaders,
            freshSession: true)!;
        var woolworthsGetRegionsResults = result!
            .Body!.StoreAreas.SelectMany(c => c.StoreAddresses)
            .Select(c => new WoolworthsGetRegionsResult(c.Id.ToString(), c.Name))
            .ToList();

        return woolworthsGetRegionsResults.Select(c => new StoreQueryResponse
        {
            StoreId = c.StoreId,
            StoreRegionName = c.StoreName,
            StoreName = StoreName.Woolworths,
        }).ToList();
    }

    private record RegionsResponse(StoreAreasResponse[] StoreAreas);

    private record StoreAreasResponse(int Id, string Name, StoreAddressesResponse[] StoreAddresses);

    private record StoreAddressesResponse(int Id, string Name);

    public async Task<CreateSessionWithRegionDto[]> CreateSessionWithRegionsAsync(string[] storeIds)
    {
        var tasks = new List<Task<CreateSessionWithRegionDto>>();

        foreach (var storeId in storeIds)
        {
            var task = CreateSessionWithRegionAsync(storeId);
            tasks.Add(task);
        }

        return await Task.WhenAll(tasks);
    }

    public async Task<CreateSessionWithRegionDto> CreateSessionWithRegionAsync(string storeId)
    {
        var url = "https://www.woolworths.co.nz/api/v1/fulfilment/my/pickup-addresses";
        var body = new
        {
            addressId = storeId
        };

        var result = await _httpHelper.PutAsync<ChangeRegionResponse>(url, body,
            headers: Headers.WoolworthsDefaultHeaders, freshSession: true)!;
        var sessionId = _httpHelper.GetCookie(url, result!.Headers, Cookies.AspNetSessionIdCookieName);
        var aga = _httpHelper.GetCookie(url, result!.Headers, Cookies.Aga);

        return new CreateSessionWithRegionDto
        {
            StoreName = StoreName.Woolworths,
            Address = result.Body!.Context.Fulfilment.Address,
            SessionId = sessionId!,
            Aga = aga!,
            StoreId = storeId,
        };
    }


    private record ChangeRegionResponse(ChangeRegionContextResponse Context);

    private record ChangeRegionContextResponse(ChangeRegionFulfillmentResponse Fulfilment);

    private record ChangeRegionFulfillmentResponse(string Address);
}