using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class ChatRoomCreatedDomainEvent : DomainEvent<Chatroom>
{
    public Chatroom CreatedEntity { get; }
    public Account CreatorAccount { get; }
    
    public ChatRoomCreatedDomainEvent(Chatroom createdEntity, Account creatorAccount)
    {
        CreatedEntity = createdEntity;
        CreatorAccount = creatorAccount;
    }
}