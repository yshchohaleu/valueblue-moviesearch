using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MongoDB.Driver;
using MovieSearch.Application.Ports;
using MovieSearch.Infrastructure.MongoDb.Config;
using MovieSearch.Infrastructure.MongoDb.Repositories;

namespace MovieSearch.Infrastructure.MongoDb;

public static class DependencyInjection
{
    public static IServiceCollection AddMongoDb(this IServiceCollection services, IConfiguration configuration)
    {
        // setup mongodb settings
        var mongoDbSettings = new MongoDbSettings();
        configuration.GetSection("MongoDbSettings").Bind(mongoDbSettings, c => c.BindNonPublicProperties = true);
        services.AddSingleton<IMongoDbSettings>(mongoDbSettings);
        
        // register mongo database
        services.AddSingleton((s) =>
        {
            var settings = s.GetRequiredService<IMongoDbSettings>();
            var mongoClientClient = new MongoClient(settings.ConnectionString);

            return mongoClientClient.GetDatabase(settings.DatabaseName);
        });
        
        // register repositories
        services.AddScoped<ISearchRequestRepository, SearchRequestRepository>();
     
        // configure entity mappings
        MovieRequestMap.Configure();

        services.AddAsyncInitializers(typeof(DependencyInjection).Assembly);

        return services;
    }
}