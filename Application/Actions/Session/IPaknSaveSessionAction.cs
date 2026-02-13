using Application.Helpers;
using Persistence;

namespace Application.Actions.Session;

public interface IPaknSaveSessionAction
{
    public Task<PaknsaveSession> GetOrCreateSessionAsync(QueriesSql dbContext);
}

class PaknSaveSessionAction : IPaknSaveSessionAction
{
    private readonly IHttpHelper _httpHelper;

    public PaknSaveSessionAction(IHttpHelper httpHelper)
    {
        _httpHelper = httpHelper;
    }

    public async Task<PaknsaveSession> GetOrCreateSessionAsync(QueriesSql dbContext)
    {
        var pakSaveSession = (await dbContext.getPaknSaveSession())?.PaknsaveSession;
        if (pakSaveSession?.AccessToken == null)
        {
            return await CreateSessionAsync(dbContext);
        }

        return pakSaveSession.Value;
    }

    private record CreateTokenResponse(string access_token);

    public async Task<PaknsaveSession> CreateSessionAsync(QueriesSql dbContext)
    {
        const string url = "https://www.paknsave.co.nz/api/user/get-current-user";
        var body = new Dictionary<string, string>()
        {
            ["fingerprintGuest"] =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/144.0.0.0 Safari/537.36",
            ["fingerprintUser"] = PaknSaveHelper.GenerateRandomHex32()
        };

        var response = await _httpHelper.PostAsync<CreateTokenResponse>(url, payload: body);
        var result = (await dbContext.createPaknSaveSession(new QueriesSql.createPaknSaveSessionArgs()
        {
            AccessToken = response.Body!.access_token,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
        }))?.PaknsaveSession;

        return result.Value;
    }
}