using Messegify.Application.Errors.Abstractions;

namespace Messegify.Application.Errors;

public class BadRequestError : ErrorException
{
    public BadRequestError(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}