using MediatR;
using Messegify.Application.Dtos;
using Messegify.Domain.Entities;

namespace Messegify.Application.Services.MessageRequests;

public class DeleteMessagesRequest : IRequest
{
    public IEnumerable<Guid> MessageIds { get; }

    public DeleteMessagesRequest(IEnumerable<MessageDto> messages)
    {
        MessageIds = messages.Select(messageDto => messageDto.Id);
    }

    public DeleteMessagesRequest(IEnumerable<Message> messages)
    {
        MessageIds = messages.Select(messageDto => messageDto.Id);
    }
}