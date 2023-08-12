using MediatR;

namespace Messegify.Application.Services.ChatroomRequests;

public class DeleteChatRoomRequest : IRequest
{
    public Guid ChatRoomId { get; }
    
    public DeleteChatRoomRequest(Guid chatRoomId)
    {
        ChatRoomId = chatRoomId;
    }
}