using MediatR;
using Messegify.Domain.Entities;
using Messegify.Domain.Events;

namespace Messegify.Application.DomainEventHandlers.ChatRoomCreated;

// ReSharper disable once UnusedType.Global
public class AddCreatorToChatRoom : INotificationHandler<ChatRoomCreatedDomainEvent>
{
    public Task Handle(ChatRoomCreatedDomainEvent notification, CancellationToken cancellationToken)
    {
        notification.CreatedEntity.Members = new List<AccountChatRoom>()
        {
            new() { AccountId = notification.CreatorAccount.Id }
        };
        
        return Task.CompletedTask;
    }
}