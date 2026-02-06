using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Enums;
using Application.Interfaces;

namespace Application.Commands.Products;

public record SyncStoreProductsRequest
{
    public StoreName[]? Stores { get; init; }
}

public class SyncStoreProductsCommand : ICommand<bool, SyncStoreProductsRequest>
{
    private readonly IWoolworthsProductAction _woolworthsProductAction;
    private readonly IPaknSaveProductAction _paknSaveProductAction;

    public SyncStoreProductsCommand(IWoolworthsProductAction woolworthsProductAction, IPaknSaveProductAction paknSaveProductAction)
    {
        _woolworthsProductAction = woolworthsProductAction;
        _paknSaveProductAction = paknSaveProductAction;
    }

    public async Task<bool> SendAsync(SyncStoreProductsRequest request)
    {
        var storesToSync = request.Stores ?? [StoreName.Woolworths, StoreName.NewWorld, StoreName.PaknSave];

        var tasks = new List<Task?>();
        
        foreach (var storeName in storesToSync)
        {
            switch (storeName)
            {
                case StoreName.Woolworths:
                {
                    var woolworthsTask = _woolworthsProductAction.SyncProductsAsync();
                    tasks.Add(woolworthsTask);
                    break;
                }
                case StoreName.PaknSave:
                {
                    var paknsaveTask =  _paknSaveProductAction.SyncProductsAsync();
                    tasks.Add(paknsaveTask);
                    break;
                }
            }
        }

        await Task.WhenAll(tasks);


        return true;
    }
}