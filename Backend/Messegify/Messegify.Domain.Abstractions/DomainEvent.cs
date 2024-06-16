using MediatR;

namespace Messegify.Domain.Abstractions;

public interface IDomainEvent : INotification;

public abstract class DomainEvent<TEntity> : IDomainEvent where TEntity : IEntity;