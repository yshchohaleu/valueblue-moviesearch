using MediatR;
using MovieSearch.Application.Ports;

namespace MovieSearch.Application.Requests.DeleteSearchRequest;

public class DeleteSearchRequestHandler : IRequestHandler<DeleteSearchRequest>
{
    private readonly ISearchRequestRepository _searchRequestRepository;

    public DeleteSearchRequestHandler(ISearchRequestRepository searchRequestRepository)
    {
        _searchRequestRepository = searchRequestRepository;
    }

    public Task Handle(DeleteSearchRequest request, CancellationToken cancellationToken)
    {
        return _searchRequestRepository.DeleteAsync(request.Id);
    }
}