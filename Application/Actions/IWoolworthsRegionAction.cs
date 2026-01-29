using Application.Constants;
using Application.Models;

namespace Application.Actions;

public interface IWoolworthsRegionAction
{
    public Task<WoolworthsChangeRegionResult> CreateSessionWithRegionAsync(int regionId);
    public Task<IList<WoolworthsGetRegionsResult>> GetRegionsAsync();
}

public record WoolworthsGetRegionsResult(int Id, string StoreName);

public record WoolworthsChangeRegionResult(string Address, string SessionId, string Aga);

public class WoolworthsRegionAction : IWoolworthsRegionAction
{
    private readonly IHttpHelper _httpHelper;

    public WoolworthsRegionAction(IHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    public async Task<IList<WoolworthsGetRegionsResult>> GetRegionsAsync()
    {
        const string url = "https://www.woolworths.co.nz/api/v1/addresses/pickup-addresses";
            var result = await _httpHelper.GetAsync<RegionsResponse?>(url, headers: Headers.WoolworthsDefaultHeaders,
                freshSession: true)!;
            var woolworthsGetRegionsResults = result!
                .Body!.StoreAreas.SelectMany(c => c.StoreAddresses)
                .Select(c => new WoolworthsGetRegionsResult(c.Id, c.Name))
                .ToList();

            return woolworthsGetRegionsResults;
    }

    private record RegionsResponse(StoreAreasResponse[] StoreAreas);

    private record StoreAreasResponse(int Id, string Name, StoreAddressesResponse[] StoreAddresses);

    private record StoreAddressesResponse(int Id, string Name);

    public async Task<WoolworthsChangeRegionResult> CreateSessionWithRegionAsync(int regionId)
    {
        var url = "https://www.woolworths.co.nz/api/v1/fulfilment/my/pickup-addresses";
        var body = new
        {
            addressId = regionId
        };

        var result = await _httpHelper.PutAsync<ChangeRegionResponse>(url, body, headers: Headers.WoolworthsDefaultHeaders, freshSession:true)!;
        var sessionId = _httpHelper.GetCookie(url, result!.Headers, Cookies.AspNetSessionIdCookieName);
        var aga = _httpHelper.GetCookie(url, result!.Headers, Cookies.Aga);

        return new WoolworthsChangeRegionResult(
            result.Body!.Context.Fulfilment.Address,
            sessionId!,
            aga!
        );
    }


    private record ChangeRegionResponse(ChangeRegionContextResponse Context);

    private record ChangeRegionContextResponse(ChangeRegionFulfillmentResponse Fulfilment);

    private record ChangeRegionFulfillmentResponse(string Address);
}