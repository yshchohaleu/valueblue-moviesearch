using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using MovieSearch.Application;
using MovieSearch.Infrastructure.MongoDb;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using MovieSearch.Api.Configuration;
using MovieSearch.Api.Middlewares;
using MovieSearch.Api.Security;
using MovieSearch.Providers.Omdb;
using MovieSearch.Shared.UserContext;

var builder = WebApplication.CreateBuilder(args);

// connect application parts
builder.Services.AddOmdb(builder.Configuration);
builder.Services.AddMongoDb(builder.Configuration);
builder.Services.AddApplication();

// register services
builder.Services.AddTransient<IUserContextProvider, UserContextProvider>();
builder.Services.AddSingleton<ApiKeyAuthorizationFilter>();
builder.Services.AddSingleton<IValidateApiKey, ApiKeyValidator>();

// register configuration
builder.Services.Configure<SecuritySettings>(builder.Configuration.GetSection(nameof(SecuritySettings)));

// enable IP address resolving
builder.Services.Configure<ForwardedHeadersOptions>(options =>
{
    options.ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto;
});

builder.Services.AddControllers();

// enable API version
builder.Services.AddApiVersioning(options =>
{
    options.DefaultApiVersion = new ApiVersion(1, 0);
    options.AssumeDefaultVersionWhenUnspecified = true;
    options.ReportApiVersions = true;
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'VVV";
    options.SubstituteApiVersionInUrl = true;
});

// configure Swagger
builder.Services.AddSwaggerGen();
builder.Services.ConfigureOptions<ConfigureSwaggerOptions>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.Configure<RouteOptions>(options =>
{
    options.LowercaseQueryStrings = true;
    options.LowercaseUrls = true;
});

var app = builder.Build();
await app.InitAsync();

// configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(options =>
    {
        //Build a swagger endpoint for each discovered API version
        foreach (var description in app.Services.GetRequiredService<IApiVersionDescriptionProvider>().ApiVersionDescriptions)
        {
            options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json",
                description.GroupName.ToUpperInvariant());
        }
    });
}

app.UseHttpsRedirection();

app.UseForwardedHeaders();
app.UseMiddleware<UserContextMiddleware>();

app.UseAuthorization();
app.UseMiddleware<ExceptionHandlerMiddleware>();
app.MapControllers();

app.Run();