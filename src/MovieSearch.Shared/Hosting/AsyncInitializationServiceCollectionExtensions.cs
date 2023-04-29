using System.Reflection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using MovieSearch.Shared.Hosting;

// ReSharper disable once CheckNamespace
namespace Microsoft.Extensions.DependencyInjection;

public static class AsyncInitializationServiceCollectionExtensions
{
    public static IServiceCollection AddAsyncInitialization(this IServiceCollection services)
    {
        services.TryAddTransient<RootInitializer>();
        return services;
    }

    public static IServiceCollection AddAsyncInitializers(this IServiceCollection services, Assembly assembly)
    {
        services.AddAsyncInitialization();

        var asyncInitializerTypes = assembly.GetTypes().Where(p => typeof(IAsyncInitializer).IsAssignableFrom(p));
        foreach (var asyncInitializerType in asyncInitializerTypes)
        {
            services.AddTransient(typeof(IAsyncInitializer), asyncInitializerType);
        }

        return services;
    }
}