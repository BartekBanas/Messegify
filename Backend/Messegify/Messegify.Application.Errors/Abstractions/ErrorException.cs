using System.Runtime.Serialization;

namespace Messegify.Application.Errors.Abstractions;

public abstract class ErrorException : Exception
{
    public ErrorException()
    {
    }

    protected ErrorException(SerializationInfo info, StreamingContext context) : base(info, context)
    {
    }

    public ErrorException(string? message) : base(message)
    {
    }

    public ErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}