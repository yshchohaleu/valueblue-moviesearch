using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MovieSearch.Api.Configuration
{
    public class ConfigureSwaggerOptions : IConfigureOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider _provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider) => _provider = provider;

        public void Configure(SwaggerGenOptions options)
        {
            foreach (var description in _provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, new OpenApiInfo
                {
                    Title = "MovieSearch API",
                    Version = description.GroupName
                });
            }

            options.OrderActionsBy((apiDesc) => $"{apiDesc.ActionDescriptor.AttributeRouteInfo.Template}_{apiDesc.HttpMethod}");
            options.SupportNonNullableReferenceTypes();

            options.OperationFilter<HeadersOperationFilter>();
        }
    }
}