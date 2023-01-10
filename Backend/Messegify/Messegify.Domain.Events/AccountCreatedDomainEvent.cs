using MediatR;
using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class AccountCreatedDomainEvent : DomainEvent<Account>
{
    public AccountCreatedDomainEvent(Account entity)
    {
        Entity = entity;
    }

    public Account Entity { get; set; }
}