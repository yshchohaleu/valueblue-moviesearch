namespace MovieSearch.Application.Entities.Requests;

public record DailyRequestStatistics(DateTime Date, long RequestCount)
{
    public static DailyRequestStatistics Zero(DateTime date) => new (date, 0);
};