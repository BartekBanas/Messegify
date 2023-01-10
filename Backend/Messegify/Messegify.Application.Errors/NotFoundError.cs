using Messegify.Application.Errors.Abstractions;

namespace Messegify.Application.Errors;

public class NotFoundError : ErrorException
{
    public NotFoundError(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}