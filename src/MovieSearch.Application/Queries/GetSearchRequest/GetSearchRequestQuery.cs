using MediatR;
using MovieSearch.Application.Entities.Requests;

namespace MovieSearch.Application.Queries.GetSearchRequest;

public record GetSearchRequestQuery (string Id) : IRequest<SearchRequest>;