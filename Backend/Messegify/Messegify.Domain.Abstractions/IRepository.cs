using System.Linq.Expressions;

namespace Messegify.Domain.Abstractions;

public interface IRepository
{
}

public interface IRepository<TEntity> : IRepository where TEntity : IEntity
{
    Task<TEntity?> GetOneAsync(params object[] guids);

    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params string[] includeProperties);

    Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>>? filter = null, params string[] includeProperties);
    Task<TEntity> GetOneRequiredAsync(Expression<Func<TEntity, bool>>? filter = null, params string[] includeProperties);
    Task<TEntity> GetOneRequiredAsync(params object[] guids);
    Task<ICollection<TEntity>> GetAllAsync();
    Task<IEnumerable<TEntity>> GetPagedAsync(int pageSize, int pageNumber);
    Task DeleteAsync(params object[] keys);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(object update, params object[] keys);
    Task SaveChangesAsync();
}