namespace MovieSearch.Application.Exceptions;

public class NotFoundException : Exception
{
    public NotFoundException(string resource) : base($"Resource '{resource}' not found")
    {
    }
}