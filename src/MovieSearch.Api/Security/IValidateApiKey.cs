namespace MovieSearch.Api.Security;

public interface IValidateApiKey
{
    Task<bool> IsValidAsync(string? apiKey);
}