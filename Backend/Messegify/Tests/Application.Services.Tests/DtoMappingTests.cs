using Messegify.Application.Dtos;
using Messegify.Domain.Entities;
using Xunit;

namespace Application.Services.Tests;

public class DtoMappingTests
{
    [Fact]
    public void MapChatroomToDto_MapsAllPropertiesCorrectly()
    {
        // Arrange
        var chatroom = new Chatroom
        {
            Id = Guid.NewGuid(),
            Name = "Test Chatroom",
            ChatRoomType = ChatRoomType.Regular,
            Members = new List<AccountChatroom>
            {
                new() { AccountId = Guid.NewGuid() },
                new() { AccountId = Guid.NewGuid() }
            }
        };

        // Act
        var dto = chatroom.ToDto();

        // Assert
        Assert.Equal(chatroom.Id, dto.Id);
        Assert.Equal(chatroom.Name, dto.Name);
        Assert.Equal(chatroom.ChatRoomType, dto.ChatRoomType);
        Assert.Equal(chatroom.Members.Select(m => m.AccountId), dto.Members);
    }

    [Fact]
    public void MapChatroomsToDtos_MapsCollectionCorrectly()
    {
        // Arrange
        var chatrooms = new List<Chatroom>
        {
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Chatroom 1",
                ChatRoomType = ChatRoomType.Regular,
                Members = new List<AccountChatroom>
                {
                    new() { AccountId = Guid.NewGuid() }
                }
            },
            new()
            {
                Id = Guid.NewGuid(),
                Name = "Test Chatroom 2",
                ChatRoomType = ChatRoomType.Direct,
                Members = new List<AccountChatroom>
                {
                    new() { AccountId = Guid.NewGuid() },
                    new() { AccountId = Guid.NewGuid() }
                }
            }
        };

        // Act
        var dtos = chatrooms.ToDto().ToList();

        // Assert
        Assert.Equal(chatrooms.Count, dtos.Count);
        for (int i = 0; i < chatrooms.Count; i++)
        {
            Assert.Equal(chatrooms[i].Id, dtos[i].Id);
            Assert.Equal(chatrooms[i].Name, dtos[i].Name);
            Assert.Equal(chatrooms[i].ChatRoomType, dtos[i].ChatRoomType);
            Assert.Equal(chatrooms[i].Members.Select(m => m.AccountId), dtos[i].Members);
        }
    }
}