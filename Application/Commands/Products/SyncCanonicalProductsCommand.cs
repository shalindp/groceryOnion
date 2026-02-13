using Application.Actions.Products;
using Application.Interfaces;
using Persistence;

namespace Application.Commands.Products;

public class SyncCanonicalProductsCommand : ICommand<bool>
{

    private readonly ICanonicalProductSyncAction _canonicalProductSyncAction;
    private readonly INpgsqlDbContext _dbContext;

    public SyncCanonicalProductsCommand(ICanonicalProductSyncAction canonicalProductSyncAction, INpgsqlDbContext dbContext)
    {
        _canonicalProductSyncAction = canonicalProductSyncAction;
        _dbContext = dbContext;
    }

    public async Task<bool> SendAsync()
    {
        var canonicalStoreProducts = await _canonicalProductSyncAction.BuildCanonicalProductsAsync(_dbContext);
        await _dbContext.Queries.CreateCanonicalStoreProducts(canonicalStoreProducts);
     
        return true;
    }
}