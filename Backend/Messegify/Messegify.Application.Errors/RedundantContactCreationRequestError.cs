using Messegify.Application.Errors.Abstractions;

namespace Messegify.Application.Errors;

public class RedundantContactCreationRequestError : ErrorException
{
    public RedundantContactCreationRequestError()
    {
        throw new BadRequestError("You are already in a contact with this user");
    }
}