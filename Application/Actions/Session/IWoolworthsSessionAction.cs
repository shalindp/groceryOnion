using Application.Constants;
using Persistence;

namespace Application.Actions.Session;

public interface IWoolworthsSessionAction
{
    public Task<List<WoolworthsSession?>> GetOrCreateSessionAsync(QueriesSql dbContext, string[] woolworthsStoreIds);
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

    public async Task<List<WoolworthsSession?>> GetOrCreateSessionAsync(QueriesSql dbContext, string[] woolworthsStoreIds)
    {
        var existingSessions = await GetSessionAsync(dbContext, woolworthsStoreIds);

        if (existingSessions.Count == woolworthsStoreIds.Length && existingSessions.All(c => woolworthsStoreIds.Contains(c?.StoreId)))
        {
            return existingSessions;
        }

        var partialExisting = woolworthsStoreIds.Where(c => !existingSessions
                .Select(o => o?.StoreId)
                .Contains(c))
            .ToArray();

        var newSessions = await CreateSessionWithRegionsAsync(dbContext, partialExisting);

        return existingSessions.Concat(newSessions).DistinctBy(c => c.Value.StoreId).ToList();
    }

    public async Task<List<WoolworthsSession?>> GetSessionAsync(QueriesSql dbContext, string[] woolworthsStoreIds)
    {
        var woolworthsSessions = (await dbContext.getWoolworthsSession(new QueriesSql.getWoolworthsSessionArgs(woolworthsStoreIds)))
            .Select(c => c.WoolworthsSession)
            .ToList();

        return woolworthsSessions;
    }

    private async Task<List<WoolworthsSession?>> CreateSessionWithRegionsAsync(QueriesSql dbContext, string[] storeIds)
    {
        var tasks = new List<Task<WoolworthSessionActionResult>>();

        foreach (var storeId in storeIds)
        {
            var task = CreateSessionWithRegionAsync(storeId);
            tasks.Add(task);
        }

        var result = (await Task.WhenAll(tasks))
            .ToList();

        return (await dbContext.CreateWoolworthsSessions(new QueriesSql.CreateWoolworthsSessionsArgs
            {
                StoreIds = result.Select(c => c.StoreId).ToArray(),
                Cookies = result.Select(c => c.Cookies).ToArray(),
                Expires = DateTime.UtcNow.AddMinutes(15)
            })).Select(c => c.WoolworthsSession)
            .ToList();
    }

    private record ChangeRegionResponse(ChangeRegionContextResponse Context);

    private record ChangeRegionContextResponse(ChangeRegionFulfillmentResponse Fulfilment);

    private record ChangeRegionFulfillmentResponse(string Address);
}