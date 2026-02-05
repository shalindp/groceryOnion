using Application.Actions;
using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Commands.Products;
using Application.Commands.Queries.Products;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Settings;

public static class ApplicationModule
{
    public static void AddToService(IServiceCollection services)
    {
        services.AddTransient<IWoolworthsRegionAction, WoolworthsRegionAction>();
        services.AddTransient<IWoolworthsProductAction, WoolworthsProductAction>();
        services.AddTransient<IPaknSaveProductAction, PaknSaveProductAction>();
        services.AddTransient<ICanonicalProductSyncAction, CanonicalProductSyncAction>();

        services.AddScoped<SyncStoreProductsCommand>();
        services.AddScoped<SyncCanonicalProductsCommand>();
        services.AddScoped<SearchProductsQuery>();
    }
}