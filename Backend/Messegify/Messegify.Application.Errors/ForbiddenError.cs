using Messegify.Application.Errors.Abstractions;

namespace Messegify.Application.Errors;

public class ForbiddenError : ErrorException
{
    public ForbiddenError()
    {
    }

    public ForbiddenError(string? message) : base(message)
    {
    }
}