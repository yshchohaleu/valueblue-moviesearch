using System.Diagnostics;
using MovieSearch.Application.Entities.Movies;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Ports;
using MovieSearch.Shared.UserContext;

namespace MovieSearch.Application.Services;

public class SearchRequestLogDecorator : ISearchMovie
{
    private readonly ISearchMovie _movieSearcherBase;
    private readonly ISearchRequestRepository _searchRequestRepository;
    private readonly IUserContextProvider _userContextProvider;

    public SearchRequestLogDecorator(
        ISearchMovie movieSearcherBase, 
        ISearchRequestRepository searchRequestRepository, 
        IUserContextProvider userContextProvider)
    {
        _movieSearcherBase = movieSearcherBase;
        _searchRequestRepository = searchRequestRepository;
        _userContextProvider = userContextProvider;
    }

    public async Task<Movie> SearchByTitleAsync(string title)
    {
        var sw = new Stopwatch();
        sw.Start();
        var result = await _movieSearcherBase.SearchByTitleAsync(title);
        sw.Stop();

        await _searchRequestRepository.SaveAsync(SearchRequest.New(
            title,
            result.ImdbId,
            sw.ElapsedMilliseconds,
            DateTime.UtcNow,
            _userContextProvider.UserContext.GetStringValueOrNull(UserContextConstants.UserProperties.IpAddress)
        ));
        
        return result;
    }
}