using MediatR;
using MovieSearch.Application.Entities.Requests;
using MovieSearch.Application.Exceptions;
using MovieSearch.Application.Ports;

namespace MovieSearch.Application.Queries.GetSearchRequest;

public class GetSearchRequestQueryHandler : IRequestHandler<GetSearchRequestQuery, SearchRequest>
{
    private readonly ISearchRequestRepository _searchRequestRepository;

    public GetSearchRequestQueryHandler(ISearchRequestRepository searchRequestRepository)
    {
        _searchRequestRepository = searchRequestRepository;
    }

    public async Task<SearchRequest> Handle(GetSearchRequestQuery requestQuery, CancellationToken cancellationToken)
    {
        var response = await _searchRequestRepository.GetByIdAsync(requestQuery.Id);
        if (response is null)
        {
            throw new NotFoundException($"Search request with Id = {requestQuery.Id}");
        }

        return response!;
    }
}