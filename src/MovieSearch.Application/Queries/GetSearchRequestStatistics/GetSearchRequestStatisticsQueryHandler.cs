﻿using MediatR;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Ports;

namespace MovieSearch.Application.Queries.GetSearchRequestStatistics;

public class GetSearchRequestStatisticsQueryHandler : IRequestHandler<GetSearchRequestStatisticsQuery, DailyRequestStatistics>
{
    private readonly ISearchRequestRepository _searchRequestRepository;

    public GetSearchRequestStatisticsQueryHandler(ISearchRequestRepository searchRequestRepository)
    {
        _searchRequestRepository = searchRequestRepository;
    }

    public async Task<DailyRequestStatistics> Handle(GetSearchRequestStatisticsQuery request, CancellationToken cancellationToken)
    {
        var statistics = await _searchRequestRepository.GetDailyStatisticsAsync(request.Date);
        return statistics!;
    }
}