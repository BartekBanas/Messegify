using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class MessageSentDomainEvent : IDomainEvent
{
    public MessageSentDomainEvent(Message entity)
    {
        Entity = entity;
    }

    public Message Entity { get; set; }
}