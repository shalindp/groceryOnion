using Application.Constants;
using Application.Models;

namespace Application.Actions.Region;

public interface IWoolworthsRegionAction
{
    public Task<Result<WoolworthsChangeRegionResult>> CreateSessionWithRegionAsync(int addressId);
    public Task<Result<IList<WoolworthsGetRegionsResult>>> GetRegionsAsync();
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

    public async Task<Result<IList<WoolworthsGetRegionsResult>>> GetRegionsAsync()
    {
        const string url = "https://www.woolworths.co.nz/api/v1/addresses/pickup-addresses";
        try
        {
            var result = await _httpHelper.GetAsync<RegionsResponse?>(url, headers: Headers.WoolworthsDefaultHeaders,
                freshSession: true)!;
            var woolworthsGetRegionsResults = result!
                .Body!.StoreAreas.SelectMany(c => c.StoreAddresses)
                .Select(c => new WoolworthsGetRegionsResult(c.Id, c.Name))
                .ToList();

            return Result<IList<WoolworthsGetRegionsResult>>.Success(woolworthsGetRegionsResults);
        }
        catch (Exception ex)
        {
            return Result<IList<WoolworthsGetRegionsResult>>.Failure($"Error fetching regions: {ex.Message}");
        }
    }

    private record RegionsResponse(StoreAreasResponse[] StoreAreas);

    private record StoreAreasResponse(int Id, string Name, StoreAddressesResponse[] StoreAddresses);

    private record StoreAddressesResponse(int Id, string Name);

    public async Task<Result<WoolworthsChangeRegionResult>> CreateSessionWithRegionAsync(int addressId)
    {
        var url = "https://www.woolworths.co.nz/api/v1/fulfilment/my/pickup-addresses?ts=" + DateTime.UtcNow.Ticks;
        var body = new
        {
            addressId
        };

        try
        {
            var headers = new Dictionary<string, string>
            {
                { "Accept", "application/json" },
                { "User-Agent", "api-client/1.0" },
                { "x-requested-with", "OnlineShopping.WebApp" }
            };

            var result = await _httpHelper.PutAsync<ChangeRegionResponse>(url, body, headers: headers)!;
            var sessionId = _httpHelper.GetCookie(url, result!.Headers, Cookies.AspNetSessionIdCookieName);
            var aga = _httpHelper.GetCookie(url, result!.Headers, Cookies.Aga);

            return Result<WoolworthsChangeRegionResult>.Success(new WoolworthsChangeRegionResult(
                result.Body!.Context.Fulfilment.Address,
                sessionId!,
                aga!
            ));
        }
        catch (Exception ex)
        {
            return Result<WoolworthsChangeRegionResult>.Failure($"Error changing region: {ex.Message}");
        }
    }


    private record ChangeRegionResponse(ChangeRegionContextResponse Context);

    private record ChangeRegionContextResponse(ChangeRegionFulfillmentResponse Fulfilment);

    private record ChangeRegionFulfillmentResponse(string Address);
}