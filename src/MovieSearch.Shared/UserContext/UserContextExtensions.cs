
namespace MovieSearch.Shared.UserContext
{
    public static class UserContextExtensions
    {
        public static string? GetStringValueOrNull(this IUserContext currentUserContext, string propertyName)
        {
            currentUserContext.Properties.TryGetValue(propertyName, out object? propertyValue);
            return propertyValue?.ToString();
        }

    }
}