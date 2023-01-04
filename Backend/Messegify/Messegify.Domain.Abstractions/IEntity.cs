using MediatR;

namespace Messegify.Domain.Abstractions;

public interface IEntity
{
    public IReadOnlyCollection<INotification> DomainEvents { get; }
    
    public void AddDomainEvent(INotification notification);
    public void ClearDomainEvents();
}