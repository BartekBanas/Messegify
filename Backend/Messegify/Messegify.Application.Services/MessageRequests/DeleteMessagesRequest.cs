using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.MessageRequests;

public class DeleteMessagesRequest : IRequest
{
    public IEnumerable<Guid> MessageIds { get; }
    
    public DeleteMessagesRequest(IEnumerable<MessageDto> messages)
    {
        MessageIds = messages.Select(messageDto => messageDto.Id);
    }
}