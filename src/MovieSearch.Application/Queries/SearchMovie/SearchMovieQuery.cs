using MediatR;
using MovieSearch.Application.Entities.Movies;

namespace MovieSearch.Application.Queries.SearchMovie;

public record SearchMovieQuery (string Title) : IRequest<Movie>;