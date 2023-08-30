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

        // Application errors
        catch (NotFoundError error)
        {
            context.Response.StatusCode = StatusCodes.Status404NotFound;
            await context.Response.WriteAsync(error.Message);
        }
        catch (UnauthorizedAccessException error)
        {
            context.Response.StatusCode = StatusCodes.Status401Unauthorized;
            await context.Response.WriteAsync(error.Message);
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
        catch (BadRequestError error)
        {
            context.Response.StatusCode = StatusCodes.Status400BadRequest;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(error.Message);
        }
        catch (ConflictError error)
        {
            context.Response.StatusCode = StatusCodes.Status409Conflict;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync(error.Message);
        }

        // Internal Server Error
        catch (Exception error)
        {
            _logger.LogError(error, error.Message);
            context.Response.StatusCode = StatusCodes.Status500InternalServerError;
            context.Response.ContentType = "text/plain";
            await context.Response.WriteAsync("Something went wrong");
        }
    }
}