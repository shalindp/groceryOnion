using Application.Actions.Products;
using Application.Actions.Regions;
using Application.Commands.Products;
using Application.Commands.User;
using Application.Queries;
using Application.Queries.User;
using Application.Services;
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

        services.AddScoped<SignInQuery>();
        services.AddScoped<CreateUserCommand>();
        services.AddScoped<SyncStoreProductsCommand>();
        services.AddScoped<SyncCanonicalProductsCommand>();
        services.AddScoped<SearchProductsQuery>();
        services.AddScoped<CreateStoreSessionsQuery>();
        services.AddScoped<RefreshTokenQuery>();

        services.AddScoped<JwtAuthenticationService>();
    }
}