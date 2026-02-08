using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Actions.Session;
using Application.Actions.User;
using Application.Commands.Products;
using Application.Commands.Stores;
using Application.Commands.User;
using Application.Queries.Product;
using Application.Queries.Store;
using Application.Queries.User;
using Application.Services;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Settings;

public static class ApplicationModule
{
    public static void AddToService(IServiceCollection services)
    {
        services.AddSingleton<ICacheService, CacheService>();
        services.AddSingleton<IWoolworthsThrottleService>(new WoolworthsThrottleService(1));
        services.AddScoped<JwtAuthenticationService>();

        services.AddScoped<IUserContext, UserContext>();
        services.AddScoped<IWoolworthsStoreAction, WoolworthsStoreAction>();
        services.AddScoped<IWoolworthsProductAction, WoolworthsProductAction>();
        services.AddScoped<IPaknSaveProductAction, PaknSaveProductAction>();
        services.AddScoped<IPaknSaveSessionAction, PaknSaveSessionAction>();
        services.AddScoped<IPaknSaveStoreAction, PaknSaveStoreAction>();
        
        services.AddScoped<ICanonicalProductSyncAction, CanonicalProductSyncAction>();

        services.AddScoped<SignInQuery>();
        services.AddScoped<CreateUserCommand>();
        services.AddScoped<SyncStoreProductsCommand>();
        services.AddScoped<SyncCanonicalProductsCommand>();
        services.AddScoped<SearchProductsQuery>();
        services.AddScoped<GetStoresQuery>();
        services.AddScoped<RefreshTokenQuery>();
        services.AddScoped<SelectStoresCommand>();
        services.AddScoped<GetProductsPricingQuery>();
    }
}