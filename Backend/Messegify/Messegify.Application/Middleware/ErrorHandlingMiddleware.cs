using FluentValidation;
using Messegify.Application.Errors;

namespace Messegify.Application.Middleware;

public class ErrorHandlingMiddleware : IMiddleware
{
    private readonly ILogger<ErrorHandlingMiddleware> _logger;

    public ErrorHandlingMiddleware(ILogger<ErrorHandlingMiddleware> logger)
    {
        _logger = logger;
    }

    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        catch (ForbiddenError error)
        {
            context.Response.StatusCode = StatusCodes.Status403Forbidden;
            await context.Response.WriteAsync(error.Message);
        }
        catch (ValidationException error)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(error.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, ex.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}