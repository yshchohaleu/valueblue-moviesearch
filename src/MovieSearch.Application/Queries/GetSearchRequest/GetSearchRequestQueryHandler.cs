using MediatR;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Ports;

namespace MovieSearch.Application.Queries.GetSearchRequest;

public class GetSearchRequestQueryHandler : IRequestHandler<GetSearchRequestQuery, SearchRequest>
{
    private readonly ISearchRequestRepository _searchRequestRepository;

    public GetSearchRequestQueryHandler(ISearchRequestRepository searchRequestRepository)
    {
        _searchRequestRepository = searchRequestRepository;
    }

    public Task<SearchRequest> Handle(GetSearchRequestQuery requestQuery, CancellationToken cancellationToken)
    {
        return _searchRequestRepository.GetByIdAsync(requestQuery.Id);
    }
}