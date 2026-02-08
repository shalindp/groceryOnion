using Application.Helpers;
using Persistence;

namespace Application.Actions.Session;

public interface IPaknSaveSessionAction
{
    public Task<PaknsaveSession> GetOrCreateSessionAsync();
    public Task<PaknsaveSession> CreateSessionAsync();

}

class PaknSaveSessionAction : IPaknSaveSessionAction
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly IHttpHelper _httpHelper;

    public PaknSaveSessionAction(INpgsqlDbContext dbContext, IHttpHelper httpHelper)
    {
        _dbContext = dbContext;
        _httpHelper = httpHelper;
    }

    public async Task<PaknsaveSession> GetOrCreateSessionAsync()
    {
        var pakSaveSession = (await _dbContext.Queries.getPaknSaveSession())?.PaknsaveSession;
        if (pakSaveSession?.AccessToken == null)
        {
            return await CreateSessionAsync();
        }

        return pakSaveSession.Value;
    }

    private record CreateTokenResponse(string access_token);

    public async Task<PaknsaveSession> CreateSessionAsync()
    {
        const string url = "https://www.paknsave.co.nz/api/user/get-current-user";
        var body = new Dictionary<string, string>()
        {
            ["fingerprintGuest"] =
                "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/144.0.0.0 Safari/537.36",
            ["fingerprintUser"] = PaknSaveHelper.GenerateRandomHex32()
        };

        var response = await _httpHelper.PostAsync<CreateTokenResponse>(url, payload: body);
        var result = (await _dbContext.Queries.createPaknSaveSession(new QueriesSql.createPaknSaveSessionArgs()
        {
            AccessToken = response.Body!.access_token,
            ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
        }))?.PaknsaveSession;

        return result.Value;
    }
}