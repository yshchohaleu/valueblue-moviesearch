using MediatR;
using MovieSearch.Application.Entities.Requests;

namespace MovieSearch.Application.Queries.GetSearchRequestStatisticsRange;

public record GetSearchRequestStatisticsRangeQuery (DateTime From, DateTime To) : IRequest<DailyRequestStatistics[]>;