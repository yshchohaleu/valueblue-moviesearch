using MediatR;
using MovieSearch.Application.Entities.Requests;

namespace MovieSearch.Application.Queries.GetSearchRequests;

public record GetSearchRequestsQuery(DateTime? From, DateTime? To, int Page, int PageSize) : IRequest<SearchRequests>;