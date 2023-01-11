using MediatR;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class ChatRoomCreatedDomainEvent : DomainEvent<ChatRoom>
{
    public ChatRoomCreatedDomainEvent(ChatRoom createdEntity, Account creatorAccount)
    {
        CreatedEntity = createdEntity;
        CreatorAccount = creatorAccount;
    }

    public ChatRoom CreatedEntity { get; }
    public Account CreatorAccount { get; }
}