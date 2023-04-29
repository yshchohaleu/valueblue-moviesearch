using MovieSearch.Application.Entities.Movies;

namespace MovieSearch.Application.Ports;

public interface ISearchMovie
{
    Task<Movie> SearchByTitleAsync(string title);
}