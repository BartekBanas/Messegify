using MediatR;

namespace Messegify.Application.Services.ChatroomRequests;

public class DeleteChatroomRequest : IRequest
{
    public Guid ChatRoomId { get; }
    
    public DeleteChatroomRequest(Guid chatRoomId)
    {
        ChatRoomId = chatRoomId;
    }
}