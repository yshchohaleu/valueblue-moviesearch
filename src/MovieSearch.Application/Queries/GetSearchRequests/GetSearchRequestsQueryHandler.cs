using MediatR;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Ports;

namespace MovieSearch.Application.Queries.GetSearchRequests;

public class GetSearchRequestsQueryHandler : IRequestHandler<GetSearchRequestsQuery, SearchRequests>
{
    private readonly ISearchRequestRepository _searchRequestRepository;

    public GetSearchRequestsQueryHandler(ISearchRequestRepository searchRequestRepository)
    {
        _searchRequestRepository = searchRequestRepository;
    }

    public async Task<SearchRequests> Handle(GetSearchRequestsQuery request, CancellationToken cancellationToken)
    {
        var result = await _searchRequestRepository.GetAsync(
                request.From,
                request.To,
                request.Page,
                request.PageSize
            );

        return new SearchRequests(result.Items, request.Page, result.Total);
    }
}