using Messegify.Domain.Entities;

namespace Messegify.Application.Dtos;

public static class MappingExtensions
{
    public static ChatRoomDto ToDto(this Chatroom chatroom)
    {
        return new ChatRoomDto
        {
            Id = chatroom.Id,
            Name = chatroom.Name,
            ChatRoomType = chatroom.ChatRoomType,
            Members = chatroom.Members.Select(x => x.AccountId)
        };
    }
    
    public static IEnumerable<ChatRoomDto> ToDto(this IEnumerable<Chatroom> chatrooms)
    {
        return chatrooms.Select(chatroom => chatroom.ToDto());
    }
}