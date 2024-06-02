using MediatR;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;

namespace Messegify.Application.DomainEventHandlers.ContactCreated;

// ReSharper disable once UnusedType.Global
public class CreateDirectMessagingChatroom : INotificationHandler<ContactCreatedDomainEvent>
{
    private readonly IRepository<Chatroom> _chatRoomRepository;

    public CreateDirectMessagingChatroom(IRepository<Chatroom> chatRoomRepository)
    {
        _chatRoomRepository = chatRoomRepository;
    }

    public async Task Handle(ContactCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        var contact = notification.CreatedEntity;

        var newChatRoom = new Chatroom()
        {
            Name = "Direct Messaging Chatroom",
            ChatRoomType = ChatRoomType.Direct,
            Members = new List<AccountChatroom>()
            {
                new() { AccountId = contact.FirstAccountId },
                new() { AccountId = contact.SecondAccountId }
            }
        };

        contact.ContactChatroom = newChatRoom;

        await _chatRoomRepository.CreateAsync(newChatRoom);
    }
}