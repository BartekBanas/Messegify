using Messegify.Application.Errors;
using Messegify.Infrastructure.Error;

namespace Messegify.Application.Middleware;

public class InfrastructureErrorHandlingMiddleware : IMiddleware
{
    public async Task InvokeAsync(HttpContext context, RequestDelegate next)
    {
        try
        {
            await next.Invoke(context);
        }
        // Infrastructure errors
        catch (ItemNotFoundErrorException error)
        {
            throw new NotFoundError(error.Message, error);
        }
        catch (ItemDuplicatedErrorException error)
        {
            throw new BadRequestError(error.Message, error);
        }
    }
}