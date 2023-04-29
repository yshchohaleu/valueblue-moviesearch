namespace MovieSearch.Application.Entities.Movies;

public record Movie(string Title,
    string Year,
    string Rated,
    string Released,
    string RunTime,
    string Genre,
    string Director,
    string Writer,
    string Actors,
    string Plot,
    string Language,
    string Country,
    string Awards,
    string Poster,
    string ImdbRating,
    string ImdbVotes,
    string ImdbId);