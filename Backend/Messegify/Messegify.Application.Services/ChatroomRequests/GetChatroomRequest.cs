using MediatR;
using Messegify.Application.Dtos;

namespace Messegify.Application.Services.ChatroomRequests;

public class GetChatroomRequest : IRequest<ChatRoomDto>
{
    public Guid Id { get; }

    public GetChatroomRequest(Guid targetChatroomId)
    {
        Id = targetChatroomId;
    }
}