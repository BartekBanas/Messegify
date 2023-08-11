using MediatR;

namespace Messegify.Application.Services.ChatRoomRequests;

public class DeleteChatRoomRequest : IRequest
{
    public Guid ChatRoomId { get; }
    
    public DeleteChatRoomRequest(Guid chatRoomId)
    {
        ChatRoomId = chatRoomId;
    }
}