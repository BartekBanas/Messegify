using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.ChatroomRequests;

public class GetChatroomRequest : IRequest<ChatRoomDto>
{
    public Guid ChatroomId { get; }

    public GetChatroomRequest(Guid targetChatroomChatroomId)
    {
        ChatroomId = targetChatroomChatroomId;
    }
}