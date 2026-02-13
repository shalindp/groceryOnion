using Application.Constants;

namespace Application.Actions.Session;

public interface IWoolworthsSessionAction
{
    public Task<WoolworthSessionActionResult[]> CreateSessionWithRegionsAsync(string[] storeId);
    public Task<WoolworthSessionActionResult> CreateSessionWithRegionAsync(string storeId);
}

public record WoolworthSessionActionResult
{
    public string StoreId { get; init; }
    public string Cookies { get; init; }
};

public class WoolworthsSessionAction : IWoolworthsSessionAction
{
    private readonly IHttpHelper _httpHelper;

    public WoolworthsSessionAction(IHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    public async Task<WoolworthSessionActionResult[]> CreateSessionWithRegionsAsync(string[] storeIds)
    {
        var tasks = new List<Task<WoolworthSessionActionResult>>();

        foreach (var storeId in storeIds)
        {
            var task = CreateSessionWithRegionAsync(storeId);
            tasks.Add(task);
        }

        return await Task.WhenAll(tasks);
    }

    public async Task<WoolworthSessionActionResult> CreateSessionWithRegionAsync(string storeId)
    {
        const string url = "https://www.woolworths.co.nz/api/v1/fulfilment/my/pickup-addresses";
        var body = new
        {
            addressId = storeId
        };

        var result = await _httpHelper.PutAsync<ChangeRegionResponse>(url, body,
            headers: Headers.WoolworthsDefaultHeaders)!;

        result.Headers.TryGetValues("Set-Cookie", out var responseCookies);

        return new WoolworthSessionActionResult
        {
            StoreId = storeId,
            Cookies = string.Join(";", responseCookies!.ToArray())
        };
    }

    private record ChangeRegionResponse(ChangeRegionContextResponse Context);

    private record ChangeRegionContextResponse(ChangeRegionFulfillmentResponse Fulfilment);

    private record ChangeRegionFulfillmentResponse(string Address);
}