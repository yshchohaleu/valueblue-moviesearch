using MovieSearch.Application.Entities.Requests;

namespace MovieSearch.Application.Ports;

public interface ISearchRequestRepository
{
    Task SaveAsync(SearchRequest searchRequest);
    Task<SearchRequest?> GetByIdAsync(string id);
    Task<SearchRequests> GetAsync(DateTime? from, DateTime? to, int page = 0, int pageSize = 10);
    Task<DailyRequestStatistics> GetDailyStatisticsAsync(DateTime date);
    Task<IReadOnlyCollection<DailyRequestStatistics>> GetDailyStatisticsAsync(DateTime from, DateTime to);
    Task DeleteAsync(string id);
}