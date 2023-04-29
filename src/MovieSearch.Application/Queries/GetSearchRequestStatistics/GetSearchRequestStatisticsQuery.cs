using MediatR;
using MovieSearch.Application.Entities.Requests;

namespace MovieSearch.Application.Queries.GetSearchRequestStatistics;

public record GetSearchRequestStatisticsQuery (DateTime Date) : IRequest<DailyRequestStatistics>;