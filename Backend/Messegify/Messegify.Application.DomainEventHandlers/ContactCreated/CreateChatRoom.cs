using MediatR;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;

namespace Messegify.Application.DomainEventHandlers.ContactCreated;

// ReSharper disable once UnusedType.Global
public class CreateChatRoom : INotificationHandler<ContactCreatedDomainEvent>
{
    private readonly IRepository<ChatRoom> _chatRoomRepository;

    public CreateChatRoom(IRepository<ChatRoom> chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task Handle(ContactCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var contact = notification.CreatedEntity;

        var newChatRoom = new ChatRoom()
        {
            Name = "DirectMessage",
            ChatRoomType = ChatRoomType.Direct,
            Members = new List<AccountChatRoom>()
            {
                new() { AccountId = contact.FirstAccountId },
                new() { AccountId = contact.SecondAccountId }
            }
        };

        await _chatRoomRepository.CreateAsync(newChatRoom);
    }
}