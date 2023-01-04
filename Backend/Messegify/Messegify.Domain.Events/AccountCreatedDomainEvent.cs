using MediatR;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class AccountCreatedDomainEvent : INotification
{
    public AccountCreatedDomainEvent(Account entity)
    {
        Entity = entity;
    }

    public Account Entity { get; set; }
}