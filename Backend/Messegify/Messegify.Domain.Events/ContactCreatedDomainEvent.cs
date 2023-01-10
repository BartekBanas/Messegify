using MediatR;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class ContactCreatedDomainEvent : DomainEvent<Contact>
{
    public ContactCreatedDomainEvent(Contact createdEntity)
    {
        CreatedEntity = createdEntity;
    }

    public Contact CreatedEntity { get; set; }
}