using Application.Actions.Region;
using Application.Products.Actions;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Settings;

public static class ApplicationModule
{
    public static void AddToService(IServiceCollection services)
    {
        services.AddTransient<IPaknSaveProductAction, PaknSaveProductAction>();
        services.AddTransient<IWoolworthsRegionAction, WoolworthsRegionAction>();
        // services.AddTransient<IWoolworthsProductAction, WoolworthsProductAction>();
    }
}