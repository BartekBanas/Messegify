using MediatR;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class ChatRoomCreatedDomainEvent : INotification
{
    public ChatRoomCreatedDomainEvent(ChatRoom createdEntity, Account creatorAccount)
    {
        CreatedEntity = createdEntity;
        CreatorAccount = creatorAccount;
    }

    public ChatRoom CreatedEntity { get; }
    public Account CreatorAccount { get; }
}