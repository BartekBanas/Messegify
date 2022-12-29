namespace Messegify.Domain.Abstractions;

public interface IRepository
{
    
}

public interface IRepository<TEntity> : IRepository where TEntity : IEntity
{
    
}