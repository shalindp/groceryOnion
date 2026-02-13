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
        return await _dbContext.WithTransactionAsync(async (dbContext) =>
        {
            if (request.WoolworthStoreIds.Length != 0)
            {
                await _woolworthsSessionAction.GetOrCreateSessionAsync(dbContext, request.WoolworthStoreIds);
            }

            if (request.PaknSaveStoreIds.Length != 0)
            {
                await _paknSaveSessionAction.GetOrCreateSessionAsync(dbContext);
            }

            return true;
        });
    }
}