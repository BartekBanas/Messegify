using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.MessageRequests;

public class GetMessagesRequest : IRequest<IEnumerable<MessageDto>>
{
    public Guid ChatRoomId { get; }
    
    public GetMessagesRequest(Guid chatRoomId)
    {
        ChatRoomId = chatRoomId;
    }
}