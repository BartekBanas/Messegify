using MediatR;

namespace Messegify.Domain.Abstractions;

public abstract class Entity : IEntity
{
    private List<INotification> _domainEvents;
    public IReadOnlyCollection<INotification> DomainEvents => _domainEvents?.AsReadOnly();
    
    public void AddDomainEvent(INotification notification)
    {
        _domainEvents ??= new List<INotification>();
        _domainEvents.Add(notification);
    }
    
    public void ClearDomainEvents()
    {
        _domainEvents?.Clear();
    }
}