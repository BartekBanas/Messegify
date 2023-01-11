namespace Messegify.Infrastructure.Error.Abstractions;

public class InfrastructureErrorException : Exception
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