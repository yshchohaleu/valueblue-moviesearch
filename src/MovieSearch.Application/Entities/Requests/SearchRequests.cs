namespace MovieSearch.Application.Entities.Requests;

public record SearchRequests(SearchRequest[] Items, int Page, long Total);