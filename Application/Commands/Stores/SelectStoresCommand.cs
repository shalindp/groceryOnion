using System.Security.Claims;
using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Actions.User;
using Application.Enums;
using Application.Interfaces;
using Persistence;

namespace Application.Commands.Stores;

public record SelectStoresCommandRequest
{
    public int[] WoolworthStoreIds { get; init; }

    public string[] PaknSaveStoreIds { get; init; }
}

public class SelectStoresCommand : ICommand<bool, SelectStoresCommandRequest>
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly IUserContext _userContext;
    private readonly IWoolworthsRegionAction _woolworthsRegionAction;
    private readonly IPaknSaveProductAction _paknSaveProductAction;


    public SelectStoresCommand(INpgsqlDbContext dbContext, IUserContext userContext, IWoolworthsRegionAction woolworthsRegionAction, IPaknSaveProductAction paknSaveProductAction)
    {
        _dbContext = dbContext;
        _userContext = userContext;
        _woolworthsRegionAction = woolworthsRegionAction;
        _paknSaveProductAction = paknSaveProductAction;
    }

    public async Task<bool> SendAsync(SelectStoresCommandRequest request)
    {
        if (request.WoolworthStoreIds.Length != 0)
        {
            var existingSessions = (await _dbContext.Queries.getWoolworthsSession(
                    new QueriesSql.getWoolworthsSessionArgs
                    {
                        AddressIds = request.WoolworthStoreIds,
                    })).Select(c => c.WoolworthsSession)
                .ToList();

            var sessionsToCreate = request.WoolworthStoreIds
                .Where(c => !existingSessions.Select(o => o?.AddressId).Contains(c))
                .ToArray();

            var newSessions = await _woolworthsRegionAction.CreateSessionWithRegionsAsync(sessionsToCreate);

            var sessionArgs = newSessions.Select(c => new QueriesSql.createWoolworthsSessionArgs()
            {
                AddressId = c.AddressId,
                SessionId = c.SessionId,
                Aga = c.Aga,
                ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
            }).ToList();

            await _dbContext.Queries.createWoolworthsSession(sessionArgs);
        }

        if (request.PaknSaveStoreIds.Length != 0)
        {
            var x = await _dbContext.Queries.getPaknSaveSession();
            if (x == null)
            {
                var accessToken = await _paknSaveProductAction.CreateAccessTokenAsync();
                await _dbContext.Queries.createPaknSaveSession(new QueriesSql.createPaknSaveSessionArgs()
                {
                    AccessToken = accessToken,
                    ExpiresUtc = DateTime.UtcNow.AddMinutes(15)
                });
            }
        }

        var selectedWoolworthsStores = request.WoolworthStoreIds
            .Select(c =>
                new QueriesSql.addSelectedStoreArgs(_userContext.UserId, StoreName.Woolworths.ToDescription(),
                    c.ToString()))
            .ToList();

        var selectedPaknSaveStores = request.PaknSaveStoreIds
            .Select(c =>
                new QueriesSql.addSelectedStoreArgs(_userContext.UserId, StoreName.Woolworths.ToDescription(), c))
            .ToList();

        var selectedStores = selectedWoolworthsStores.Concat(selectedPaknSaveStores).ToList();

        await _dbContext.Queries.addSelectedStore(selectedStores);

        return true;
    }
}