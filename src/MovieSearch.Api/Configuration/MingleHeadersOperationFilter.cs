using Microsoft.OpenApi.Any;
using Microsoft.OpenApi.Models;
using MovieSearch.Api.Security;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace MovieSearch.Api.Configuration
{
    public sealed class HeadersOperationFilter : IOperationFilter
    {
        public void Apply(OpenApiOperation operation, OperationFilterContext context)
        {
            var filterPipeline = context.ApiDescription.ActionDescriptor.FilterDescriptors;
            var isAuthorized = filterPipeline.Select(filterInfo => filterInfo.Filter).Any(filter => filter is ApiKeyAttribute);

            if (isAuthorized)
            {
                operation.Parameters ??= new List<OpenApiParameter>();
                operation.Parameters.Add(new OpenApiParameter
                {
                    Name = "X-API-Key",
                    In = ParameterLocation.Header,
                    Description = "API Key",
                    Examples = new Dictionary<string, OpenApiExample>
                    {
                        { "TST API Key", new OpenApiExample { Description = "TST API Key", Value = new OpenApiString("01B98C12-42AF-4A95-BAD9-AD89C5977D35") }}
                    },
                    Required = true
                });
            }
        }
    }
}