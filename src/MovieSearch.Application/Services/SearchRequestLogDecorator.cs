﻿using System.Diagnostics;
using Microsoft.Extensions.Logging;
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
    private readonly ILogger<SearchRequestLogDecorator> _logger;

    public SearchRequestLogDecorator(
        ISearchMovie movieSearcherBase, 
        ISearchRequestRepository searchRequestRepository, 
        IUserContextProvider userContextProvider, 
        ILogger<SearchRequestLogDecorator> logger)
    {
        _movieSearcherBase = movieSearcherBase;
        _searchRequestRepository = searchRequestRepository;
        _userContextProvider = userContextProvider;
        _logger = logger;
    }

    public async Task<Movie> SearchByTitleAsync(string title)
    {
        var sw = new Stopwatch();
        sw.Start();
        var result = await _movieSearcherBase.SearchByTitleAsync(title);
        sw.Stop();

        try
        {
            await _searchRequestRepository.SaveAsync(SearchRequest.New(
                title,
                result.ImdbId,
                sw.ElapsedMilliseconds,
                DateTime.UtcNow,
                _userContextProvider.UserContext.GetStringValueOrNull(UserContextConstants.UserProperties.IpAddress)
            ));
        }
        catch (Exception e)
        {
            _logger.LogError(e,  "Error during saving request");
        }
        
        return result;
    }
}