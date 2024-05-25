using MediatR;
using Messegify.Domain.Entities;

namespace Messegify.Application.Services.ChatroomRequests;

public class CreateChatroomRequest : IRequest
{
    public ChatRoomType ChatRoomType { get; }
    public string? Name { get; }

    public CreateChatroomRequest(ChatRoomType chatRoomType, string? name = null)
    {
        ChatRoomType = chatRoomType;
        Name = name;
    }
}