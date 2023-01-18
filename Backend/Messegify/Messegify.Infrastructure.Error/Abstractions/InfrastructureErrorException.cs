namespace Messegify.Infrastructure.Error.Abstractions;

public abstract class InfrastructureErrorException : Exception
{
    public InfrastructureErrorException()
    {
    }

    public InfrastructureErrorException(string? message) : base(message)
    {
    }

    public InfrastructureErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }
}