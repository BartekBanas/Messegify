using Messegify.Domain.Abstractions;
using Messegify.Domain.Entities;

namespace Messegify.Domain.Events;

public class AccountCreatedDomainEvent : DomainEvent<Account>
{
    public Account Entity { get; set; }
    
    public AccountCreatedDomainEvent(Account entity)
    {
        Entity = entity;
    }
}