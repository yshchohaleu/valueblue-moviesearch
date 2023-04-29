using System.Net;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MovieSearch.Application.Ports;
using MovieSearch.Providers.Omdb.Client;
using Polly;
using Polly.Extensions.Http;

namespace MovieSearch.Providers.Omdb;

public static class DependencyInjection
{
    public static IServiceCollection AddOmdb(this IServiceCollection services, IConfiguration configuration)
    {
        services.Configure<OmdbSettings>(configuration.GetSection(nameof(OmdbSettings)));
        
        services.AddScoped<ISearchMovie, OmdbClient>();
        services.AddHttpClient<ISearchMovie, OmdbClient>(httpClient =>
        {
            var omdbSettings = new OmdbSettings();
            configuration.GetSection(nameof(OmdbSettings)).Bind(omdbSettings);

            httpClient.BaseAddress = new Uri(omdbSettings.BaseUrl);
            httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
        })
            .AddPolicyHandler(GetRetryPolicy());
        
        static IAsyncPolicy<HttpResponseMessage> GetRetryPolicy()
        {
            return HttpPolicyExtensions
                .HandleTransientHttpError()
                .OrResult(msg => msg.StatusCode == HttpStatusCode.InternalServerError)
                .WaitAndRetryAsync(5, retryAttempt => TimeSpan.FromSeconds(Math.Pow(2,
                    retryAttempt)));
        }

        return services;
    }
}