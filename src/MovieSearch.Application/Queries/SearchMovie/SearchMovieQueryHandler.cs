using MediatR;
using MovieSearch.Application.Entities.Movies;
using MovieSearch.Application.Ports;

namespace MovieSearch.Application.Queries.SearchMovie;

public class SearchMovieQueryHandler : IRequestHandler<SearchMovieQuery, Movie>
{
    private readonly ISearchMovie _movieSearcher;
    
    public SearchMovieQueryHandler(ISearchMovie movieSearcher)
    {
        _movieSearcher = movieSearcher;
    }

    public Task<Movie> Handle(SearchMovieQuery query, CancellationToken cancellationToken)
    {
        return _movieSearcher.SearchByTitleAsync(query.Title);
    }
}