using MovieSearch.Shared.UserContext;

namespace MovieSearch.Api.Middlewares;

internal sealed class UserContextMiddleware
{
    private readonly RequestDelegate _next;

    public UserContextMiddleware(RequestDelegate next)
    {
        _next = next;
    }

    public async Task InvokeAsync(HttpContext httpContext, IUserContextProvider userContextProvider)
    {
        if (userContextProvider is not UserContextProvider userContextProviderImp)
        {
            throw new ApplicationException(
                "This middleware requires `UserContextProvider` to implement `IUserContextProvider` interface");
        }

        var properties = new Dictionary<string, object>();
        var ipAddress = GetUserIpAddress(httpContext);
        if (!string.IsNullOrEmpty(ipAddress))
            properties.Add(UserContextConstants.UserProperties.IpAddress, ipAddress);

        userContextProviderImp.Initialize(UserContext.Create(properties));
        
        await _next(httpContext!);
    }

    private static string? GetUserIpAddress(HttpContext httpContext)
    {
        return httpContext.Request.HttpContext.Connection.RemoteIpAddress?.ToString();
    }
}