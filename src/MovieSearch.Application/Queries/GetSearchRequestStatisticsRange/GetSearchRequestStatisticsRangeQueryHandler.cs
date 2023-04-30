using MediatR;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Ports;

namespace MovieSearch.Application.Queries.GetSearchRequestStatisticsRange;

public class GetSearchRequestStatisticsRangeQueryHandler : IRequestHandler<GetSearchRequestStatisticsRangeQuery, DailyRequestStatistics[]>
{
    private readonly ISearchRequestRepository _searchRequestRepository;

    public GetSearchRequestStatisticsRangeQueryHandler(ISearchRequestRepository searchRequestRepository)
    {
        _searchRequestRepository = searchRequestRepository;
    }

    public async Task<DailyRequestStatistics[]> Handle(GetSearchRequestStatisticsRangeQuery request, CancellationToken cancellationToken)
    {
        var statistics = await _searchRequestRepository.GetDailyStatisticsAsync(request.From, request.To);
        return statistics.ToArray();
    }
}