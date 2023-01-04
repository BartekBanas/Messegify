using MediatR;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class ContactCreatedDomainEvent : INotification
{
    public ContactCreatedDomainEvent(Contact createdEntity)
    {
        CreatedEntity = createdEntity;
    }

    public Contact CreatedEntity { get; set; }
}