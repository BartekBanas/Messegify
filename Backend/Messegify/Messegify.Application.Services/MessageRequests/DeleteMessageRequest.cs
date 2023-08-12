using MediatR;

namespace Messegify.Application.Services.MessageRequests;

public class DeleteMessageRequest : IRequest
{
    public Guid MessageId { get; set; }

    public DeleteMessageRequest(Guid messageId)
    {
        MessageId = messageId;
    }
}