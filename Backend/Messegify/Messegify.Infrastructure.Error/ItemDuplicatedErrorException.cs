using Messegify.Infrastructure.Error.Abstractions;

namespace Messegify.Infrastructure.Error;

public class ItemDuplicatedErrorException : InfrastructureErrorException
{
    public ItemDuplicatedErrorException(string? message) : base(message)
    {
    }

    public ItemDuplicatedErrorException(string? message, Exception? innerException) : base(message, innerException)
    {
    }

    public ItemDuplicatedErrorException()
    {
    }
}