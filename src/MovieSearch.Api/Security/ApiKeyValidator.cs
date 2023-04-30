using Microsoft.Extensions.Options;

namespace MovieSearch.Api.Security;

public class ApiKeyValidator : IValidateApiKey
{
    private readonly IOptions<SecuritySettings> _securitySettings;

    public ApiKeyValidator(IOptions<SecuritySettings> securitySettings)
    {
        _securitySettings = securitySettings;
    }

    public Task<bool> IsValidAsync(string? apiKey)
    {
        if (string.IsNullOrWhiteSpace(apiKey))
            return Task.FromResult(false);
        
        return Task.FromResult(
            _securitySettings.Value.ApiKeys
                .Contains(apiKey, StringComparer.InvariantCultureIgnoreCase));
    }
}