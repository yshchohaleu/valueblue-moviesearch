using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace MovieSearch.Api.Security;

public class ApiKeyAuthorizationFilter : IAsyncAuthorizationFilter
{
    private const string ApiKeyHeaderName = "X-API-Key";

    private readonly IValidateApiKey _apiKeyValidator;

    public ApiKeyAuthorizationFilter(IValidateApiKey apiKeyValidator)
    {
        _apiKeyValidator = apiKeyValidator;
    }

    public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
    {
        string? apiKey = context.HttpContext.Request.Headers[ApiKeyHeaderName];
        var isValid = await _apiKeyValidator.IsValidAsync(apiKey);
        
        if (!isValid)
        {
            context.Result = new UnauthorizedResult();
        }
    }
}