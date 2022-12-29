namespace Messegify.Domain.Abstractions;

public interface IRepository
{
    
}

public interface IRepository<TEntity> where TEntity : IEntity
{
    
}