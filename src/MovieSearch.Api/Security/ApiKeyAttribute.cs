using Microsoft.AspNetCore.Mvc;

namespace MovieSearch.Api.Security;

public class ApiKeyAttribute : ServiceFilterAttribute
{
    public ApiKeyAttribute()
        : base(typeof(ApiKeyAuthorizationFilter))
    {
    }
}