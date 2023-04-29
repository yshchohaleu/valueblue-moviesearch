using System.Net.Http.Json;
using Microsoft.Extensions.Options;
using MovieSearch.Application.Entities.Movies;
using MovieSearch.Application.Exceptions;
using MovieSearch.Application.Ports;

namespace MovieSearch.Providers.Omdb.Client;

public class OmdbClient : ISearchMovie
{
    private readonly HttpClient _httpClient;
    private readonly IOptions<OmdbSettings> _settings;

    public OmdbClient(HttpClient httpClient, IOptions<OmdbSettings> settings)
    {
        _httpClient = httpClient;
        _settings = settings;
    }

    public async Task<Movie> SearchByTitleAsync(string title)
    {
        var httpRequestMessage = new HttpRequestMessage
        {
            Method = HttpMethod.Get,
            RequestUri = new Uri($"/?t={title}&apikey={_settings.Value.ApiKey}", UriKind.Relative)
        };
        
        var response = await _httpClient.SendAsync(httpRequestMessage);
        response.EnsureSuccessStatusCode();
        
        var movieResponse = await response.Content.ReadFromJsonAsync<MovieResponse>();
        if (movieResponse == null 
            || !string.Equals(movieResponse.Response, "True", StringComparison.InvariantCultureIgnoreCase))
        {
            throw new NotFoundException($"Movie with title {title}"); 
        }

        return movieResponse.ToDomain();
    }
}