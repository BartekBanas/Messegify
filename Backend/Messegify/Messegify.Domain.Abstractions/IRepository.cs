﻿using System.Linq.Expressions;

namespace Messegify.Domain.Abstractions;

public interface IRepository;

public interface IRepository<TEntity> : IRepository where TEntity : IEntity
{
    Task<TEntity?> GetOneAsync(params object[] keys);

    Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params string[] includeProperties);

    Task<IEnumerable<TEntity>> GetAsync(
        int pageSize, int pageNumber,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params string[] includeProperties);

    Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>>? filter = null, params string[] includeProperties);
    Task<TEntity> GetOneRequiredAsync(Expression<Func<TEntity, bool>>? filter = null, params string[] includeProperties);
    Task<TEntity> GetOneRequiredAsync(object key, params string[] includeProperties);
    Task<TEntity> GetOneRequiredAsync(params object[] keys);
    Task DeleteOneAsync(params object[] keys);
    Task<TEntity> CreateAsync(TEntity entity);
    Task<TEntity> UpdateAsync(object update, params object[] keys);
    Task SaveChangesAsync();
}