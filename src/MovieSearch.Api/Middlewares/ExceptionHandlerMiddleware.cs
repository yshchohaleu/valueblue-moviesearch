using System.Net;
using MovieSearch.Application.Exceptions;
using Newtonsoft.Json;

namespace MovieSearch.Api.Middlewares;

public class ExceptionHandlerMiddleware
{
    private readonly ILogger<ExceptionHandlerMiddleware> _logger;
    private readonly RequestDelegate _next;
    
    public ExceptionHandlerMiddleware(RequestDelegate next, ILogger<ExceptionHandlerMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (Exception exception)
        {
            _logger.LogError(exception, "Error executing {Context}", context.Request.Path.Value);
            var response = context.Response;
            response.ContentType = "application/json";

            var (status, message) = GetResponse(exception);
            response.StatusCode = (int)status;
            await response.WriteAsync(message);
        }
    }
    
    private (HttpStatusCode code, string message) GetResponse(Exception exception)
    {
        HttpStatusCode code;
        switch (exception)
        {
            case NotFoundException:
                code = HttpStatusCode.NotFound;
                break;
            case ArgumentNullException
                or ArgumentOutOfRangeException:
                code = HttpStatusCode.BadRequest;
                break;
            default:
                code = HttpStatusCode.InternalServerError;
                break;
        }

        return (code, JsonConvert.SerializeObject(new { exception.Message }));
    }
}