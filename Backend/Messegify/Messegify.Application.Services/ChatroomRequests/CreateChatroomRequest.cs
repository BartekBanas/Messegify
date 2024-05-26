using MediatR;
using Messegify.Application.Dtos;
using Messegify.Domain.Entities;

namespace Messegify.Application.Services.ChatroomRequests;

public class CreateChatroomRequest : IRequest, IRequest<ChatRoomDto>
{
    public ChatRoomType ChatRoomType { get; }
    public string? Name { get; }

    public CreateChatroomRequest(ChatRoomType chatRoomType, string? name = null)
    {
        ChatRoomType = chatRoomType;
        Name = name;
    }
}