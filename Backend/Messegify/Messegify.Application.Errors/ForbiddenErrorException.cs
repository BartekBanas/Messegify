using Messegify.Application.Errors.Abstractions;

namespace Messegify.Application.Errors;

public class ForbiddenErrorException : ErrorException
{
    public ForbiddenErrorException()
    {
    }

    public ForbiddenErrorException(string? message) : base(message)
    {
    }
}