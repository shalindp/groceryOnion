using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Actions.Session;
using Application.Actions.User;
using Application.Interfaces;
using Persistence;

namespace Application.Commands.Stores;

public record SelectStoresCommandRequest
{
    public string[] WoolworthStoreIds { get; init; }

    public string[] PaknSaveStoreIds { get; init; }
}

public class SelectStoresCommand : ICommand<bool, SelectStoresCommandRequest>
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly IPaknSaveSessionAction _paknSaveSessionAction;
    private readonly IWoolworthsSessionAction _woolworthsSessionAction;

    public SelectStoresCommand(INpgsqlDbContext dbContext, IUserContext userContext, IPaknSaveSessionAction paknSaveSessionAction, IWoolworthsSessionAction woolworthsSessionAction)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _paknSaveSessionAction = paknSaveSessionAction;
        _woolworthsSessionAction = woolworthsSessionAction;
    }


    public async Task<bool> SendAsync(SelectStoresCommandRequest request)
    {
        if (request.WoolworthStoreIds.Length != 0)
        {
            var existingSessions = (await _dbContext.Queries.getWoolworthsSession(
                    new QueriesSql.getWoolworthsSessionArgs
                    {
                        StoreIds = ["awwa"]
                    })).Select(c => c.WoolworthsSession)
                .ToList();

            var sessionsToCreate = request.WoolworthStoreIds
                .Where(c => !existingSessions.Select(o => o?.StoreId).Contains(c))
                .ToArray();

            var newSessions = await _woolworthsSessionAction.CreateSessionWithRegionsAsync(sessionsToCreate);

            var sessionArgs = newSessions.Select(c => new QueriesSql.createWoolworthsSessionArgs()
            {
                StoreId = c.StoreId,
                Cookies = c.Cookies,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
            }).ToList();

            await _dbContext.Queries.createWoolworthsSession(sessionArgs);
        }

        if (request.PaknSaveStoreIds.Length != 0)
        {
            var x = await _dbContext.Queries.getPaknSaveSession();
            if (x == null)
            {
                await _paknSaveSessionAction.GetOrCreateSessionAsync();
            }
        }

        // var selectedWoolworthsStores = request.WoolworthStoreIds
        //     .Select(c =>
        //         new QueriesSql.addSelectedStoreArgs(_userContext.UserId, StoreName.Woolworths.ToDescription(),
        //             c.ToString()))
        //     .ToList();
        //
        // var selectedPaknSaveStores = request.PaknSaveStoreIds
        //     .Select(c =>
        //         new QueriesSql.addSelectedStoreArgs(_userContext.UserId, StoreName.PaknSave.ToDescription(), c))
        //     .ToList();

        // var selectedStores = selectedWoolworthsStores.Concat(selectedPaknSaveStores).ToList();

        // await _dbContext.Queries.addSelectedStore(selectedStores);

        return true;
    }
}