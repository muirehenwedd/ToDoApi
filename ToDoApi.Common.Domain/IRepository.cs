using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Common.Domain;

public interface IRepository<TEntity, in TKey> where TEntity : class
{
    public DbContext DatabaseContext { get; }
    public IQueryable<TEntity> Query { get; }

    Task Create(TEntity item);
    Task<TEntity?> GetById(TKey id);
    Task<IEnumerable<TEntity>> GetAll();
    Task Delete(TEntity item);
    Task Update(TEntity item);

    virtual async Task<TEntity?> FirstOrDefault(Expression<Func<TEntity, bool>> predicate)
    {
        return await Query.FirstOrDefaultAsync(predicate);
    }

    virtual async Task<IEnumerable<TEntity>> GetWhere(Expression<Func<TEntity, bool>> predicate)
    {
        return await Query
            .Where(predicate)
            .ToListAsync();
    }

    virtual async Task<bool> Any(Expression<Func<TEntity, bool>> predicate)
    {
        return await Query.AnyAsync(predicate);
    }

    virtual async Task CommitAsync(Func<Task> action)
    {
        await using var transaction = await DatabaseContext.Database.BeginTransactionAsync();

        try
        {
            await action();
            await DatabaseContext.SaveChangesAsync();
            await transaction.CommitAsync();
        }
        catch (Exception)
        {
            await transaction.RollbackAsync();
            throw;
        }
    }

    virtual async Task CommitAsync(Task task)
    {
        await CommitAsync(() => task);
    }
}