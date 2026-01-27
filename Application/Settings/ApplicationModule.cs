using Application.Actions.Products;
using Application.Actions.Region;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Settings;

public static class ApplicationModule
{
    public static void AddToService(IServiceCollection services)
    {
        services.AddTransient<IWoolworthsRegionAction, WoolworthsRegionAction>();
        services.AddTransient<IWoolworthsProductAction, WoolworthsProductAction>();
    }
}