using Messegify.Application.Errors.Abstractions;

namespace Messegify.Application.Errors;

public class ConflictError : ErrorException
{
    public ConflictError(string? message, Exception? innerException) : base(message, innerException)
    {
        
    }
}