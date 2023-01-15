using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.ChatRoomRequests;

public class GetMessagesRequest : IRequest<IEnumerable<MessageDto>>
{
    public GetMessagesRequest(Guid chatRoomId)
    {
        ChatRoomId = chatRoomId;
    }

    public Guid ChatRoomId { get; }
}