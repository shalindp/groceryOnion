using Application.Actions.Products;
using Application.Enums;
using Application.Interfaces;
using Persistence;

namespace Application.Commands.Products;

public record SyncStoreProductsRequest
{
    public StoreName[]? Stores { get; init; }
}

public class SyncStoreProductsCommand : ICommand<bool, SyncStoreProductsRequest>
{
    private readonly INpgsqlDbContext _dbContext;
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IPaknSaveProductAction _paknSaveProductAction;

    public SyncStoreProductsCommand(IWoolworthsProductAction woolworthsProductAction, IPaknSaveProductAction paknSaveProductAction, INpgsqlDbContext dbContext)
    {
        _woolworthsProductAction = woolworthsProductAction;
        _paknSaveProductAction = paknSaveProductAction;
        _dbContext = dbContext;
    }

    public async Task<bool> SendAsync(SyncStoreProductsRequest request)
    {
        var storesToSync = request.Stores ?? [StoreName.Woolworths, StoreName.NewWorld, StoreName.PaknSave];

        return await _dbContext.WithTransactionAsync(async (queriesSql) =>
        {
            var tasks = new List<Task<QueriesSql.CreateProductsArgs[]>>();

            foreach (var storeName in storesToSync)
            {
                switch (storeName)
                {
                    case StoreName.Woolworths:
                    {
                        var woolworthsTask = _woolworthsProductAction.GetStoreProductsAsync(queriesSql);
                        tasks.Add(woolworthsTask);
                        break;
                    }
                    case StoreName.PaknSave:
                    {
                        var paknSaveTask = _paknSaveProductAction.GetStoreProductsAsync(queriesSql);
                        tasks.Add(paknSaveTask);
                        break;
                    }
                }
            }

            var finalProducts = (await Task.WhenAll(tasks)).SelectMany(c => c).ToList();

            await _dbContext.Queries.CreateProducts(finalProducts);

            return true;
        });
    }
}