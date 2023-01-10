using MediatR;

namespace Messegify.Domain.Abstractions;

public abstract class Entity : IEntity
{
    private List<IDomainEvent> _domainEvents;
    public IReadOnlyCollection<IDomainEvent> DomainEvents => _domainEvents?.AsReadOnly();
    
    public void AddDomainEvent(IDomainEvent notification)
    {
        _domainEvents ??= new List<IDomainEvent>();
        _domainEvents.Add(notification);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}