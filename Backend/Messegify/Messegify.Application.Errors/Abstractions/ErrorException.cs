namespace Messegify.Application.Errors.Abstractions;

public abstract class ErrorException : Exception
{
    public ErrorException()
    {
    }

    public ErrorException(string? message) : base(message)
    {
    }

    public ErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}