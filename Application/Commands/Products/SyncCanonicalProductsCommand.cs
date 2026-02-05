using Application.Actions.Products;
using Application.Interfaces;

namespace Application.Commands.Products;

public class SyncCanonicalProductsCommand : ICommand<bool>
{

    private readonly IProductsAction _productsAction;

    public SyncCanonicalProductsCommand(IProductsAction productsAction)
    {
        _productsAction = productsAction;
    }

    public async Task<bool> SendAsync()
    {
        await _productsAction.SyncToCanonicalProducts();
        return true;
    }
}