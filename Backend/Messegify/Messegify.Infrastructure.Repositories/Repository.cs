﻿using System.Linq.Expressions;
using MediatR;
using Messegify.Domain.Abstractions;
using Messegify.Infrastructure.Error;
using Microsoft.EntityFrameworkCore;

namespace Messegify.Infrastructure.Repositories;

public class Repository<TEntity, TDbContext> : IRepository<TEntity>
    where TEntity : class, IEntity
    where TDbContext : DbContext
{
    private readonly IMediator _mediator;
    private readonly DbContext _dbContext;
    private readonly DbSet<TEntity> _dbSet;
    private readonly Func<Task> _saveChangesAsyncDelegate;

    public Repository(TDbContext dbContext, IMediator mediator)
    {
        _mediator = mediator;

        _dbContext = dbContext;
        _dbSet = _dbContext.Set<TEntity>();

        _saveChangesAsyncDelegate = async () => { await dbContext.SaveChangesAsync(); };
    }

    public virtual async Task<TEntity?> GetOneAsync(params object[] keys)
    {
        if (keys.Length == 0)
            throw new ArgumentException("No key provided");

        var entity = await _dbSet.FindAsync(keys);

        return entity;
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params string[] includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            return await orderBy(query).ToListAsync();
        }
        else
        {
            return await query.ToListAsync();
        }
    }
    
    public virtual async Task<IEnumerable<TEntity>> GetAsync(
        int pageSize, int pageNumber,
        Expression<Func<TEntity, bool>>? filter = null,
        Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>>? orderBy = null,
        params string[] includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        if (orderBy != null)
        {
            query = orderBy(query);
        }

        var totalCount = await query.CountAsync();

        // Calculate the starting index for paging backwards
        var startPageIndex = Math.Max(totalCount - (pageNumber + 1) * pageSize, 0);

        // Query the data based on the starting index and page size
        query = query.Skip(startPageIndex).Take(pageSize);

        return await query.ToListAsync();
    }

    public async Task<TEntity?> GetOneAsync(Expression<Func<TEntity, bool>>? filter = null,
        params string[] includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.FirstOrDefaultAsync();
    }

    public async Task<TEntity> GetOneRequiredAsync(Expression<Func<TEntity, bool>>? filter = null, params string[] includeProperties)
    {
        IQueryable<TEntity> query = _dbSet;

        if (filter != null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties)
        {
            query = query.Include(includeProperty);
        }

        return await query.FirstOrDefaultAsync() ?? throw new ItemNotFoundErrorException();
    }

    public virtual async Task<TEntity> GetOneRequiredAsync(object key, params string[] includeProperties)
    {
        var entity = await _dbSet.FindAsync(key);

        if (entity == null)
        {
            throw new ItemNotFoundErrorException();
        }

        foreach (var includeProperty in includeProperties)
        {
            await _dbContext.Entry(entity).Reference(includeProperty).LoadAsync();
        }

        return entity;
    }

    public virtual async Task<TEntity> GetOneRequiredAsync(params object[] keys)
    {
        var entity = await GetOneAsync(keys);

        if (entity == null)
             throw new ItemNotFoundErrorException(); 

        return entity;
    }

    public virtual async Task DeleteOneAsync(params object[] keys)
    {
        var entity = await GetOneRequiredAsync(keys);

        _dbSet.Remove(entity);
    }

    public virtual Task<TEntity> CreateAsync(TEntity entity)
    {
        _dbSet.Add(entity);

        return Task.FromResult(entity);
    }

    public virtual async Task<TEntity> UpdateAsync(object update, params object[] keys)
    {
        var entity = await GetOneRequiredAsync(keys);

        _dbSet.Attach(entity).CurrentValues.SetValues(update);
        _dbSet.Attach(entity).State = EntityState.Modified;

        return entity;
    }

    public virtual async Task SaveChangesAsync()
    {
        await _mediator.DispatchDomainEventsAsync(_dbContext);

        await _saveChangesAsyncDelegate();
    }
}