using MediatR;

namespace MovieSearch.Application.Requests.DeleteSearchRequest;

public record DeleteSearchRequest(string Id) : IRequest;