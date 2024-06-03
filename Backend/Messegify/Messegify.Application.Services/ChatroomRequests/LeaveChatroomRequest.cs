using MediatR;

namespace Messegify.Application.Services.ChatroomRequests;

public class LeaveChatroomRequest : IRequest
{
    public Guid ChatroomId { get; }

    public LeaveChatroomRequest(Guid chatroomId)
    {
        ChatroomId = chatroomId;
    }
}