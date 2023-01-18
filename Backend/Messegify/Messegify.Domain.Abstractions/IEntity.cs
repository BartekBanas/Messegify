using MediatR;

namespace Messegify.Domain.Abstractions;

public interface IEntity
{
    public IReadOnlyCollection<IDomainEvent>? DomainEvents { get; }
    
    public void AddDomainEvent(IDomainEvent notification);
    public void ClearDomainEvents();
}