using Application.Actions.Products;
using Application.Interfaces;

namespace Application.Commands.Products;

public class SyncCanonicalProductsCommand : ICommand<bool>
{

    private readonly ICanonicalProductSyncAction _canonicalProductSyncAction;

    public SyncCanonicalProductsCommand(ICanonicalProductSyncAction canonicalProductSyncAction)
    {
        _canonicalProductSyncAction = canonicalProductSyncAction;
    }

    public async Task<bool> SendAsync()
    {
        await _canonicalProductSyncAction.SyncToCanonicalProducts();
        return true;
    }
}