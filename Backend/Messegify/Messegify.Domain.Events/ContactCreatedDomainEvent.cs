using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class ContactCreatedDomainEvent : DomainEvent<Contact>
{
    public Contact CreatedEntity { get; set; }
    
    public ContactCreatedDomainEvent(Contact createdEntity)
    {
        CreatedEntity = createdEntity;
    }
}