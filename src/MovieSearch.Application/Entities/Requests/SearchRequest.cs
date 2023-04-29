using MovieSearch.Shared.Domain;
namespace MovieSearch.Application.Entities.Requests;

public record SearchRequest(
    string Id,
    string SearchToken,
    string ImdbId,
    long ProcessingTimeMs,
    DateTime Timestamp,
    string? IpAddress) : IEntity
{
    public static SearchRequest New(
        string searchToken,
        string imdbId,
        long processingTimeMs,
        DateTime timestamp,
        string? ipAddress)
    {
        _ = !string.IsNullOrWhiteSpace(searchToken) ? searchToken : throw new ArgumentNullException(nameof(searchToken));
        _ = !string.IsNullOrWhiteSpace(imdbId) ? imdbId : throw new ArgumentNullException(nameof(imdbId));
        _ = processingTimeMs > 0 ? processingTimeMs : throw new ArgumentOutOfRangeException(nameof(processingTimeMs));

        return new SearchRequest(
            null!,
            searchToken,
            imdbId,
            processingTimeMs,
            timestamp,
            ipAddress);
    }
};