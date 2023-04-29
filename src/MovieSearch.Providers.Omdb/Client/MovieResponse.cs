using MovieSearch.Application.Entities.Movies;

namespace MovieSearch.Providers.Omdb.Client;

public record MovieResponse(
    string? Title,
    string? Year,
    string? Rated,
    string? Released,
    string? RunTime,
    string? Genre,
    string? Director,
    string? Writer,
    string? Actors,
    string? Plot,
    string? Language,
    string? Country,
    string? Awards,
    string? Poster,
    string? ImdbRating,
    string? ImdbVotes,
    string? ImdbId,
    string? Error,
    string Response)
{
    public Movie ToDomain()
    {
        return new Movie(
            Title!,
            Year!,
            Rated!,
            Released!,
            RunTime!,
            Genre!,
            Director!,
            Writer!,
            Actors!,
            Plot!,
            Language!,
            Country!,
            Awards!,
            Poster!,
            ImdbRating!,
            ImdbVotes!,
            ImdbId!
        );
    }
};