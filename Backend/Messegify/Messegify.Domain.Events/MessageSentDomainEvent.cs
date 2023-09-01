using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class MessageSentDomainEvent : IDomainEvent
{
    public Message Entity { get; set; }
    
    public MessageSentDomainEvent(Message entity)
    {
        Entity = entity;
    }
}