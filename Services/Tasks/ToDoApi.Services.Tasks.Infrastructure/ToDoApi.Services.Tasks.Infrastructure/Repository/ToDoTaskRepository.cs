using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Services.Tasks.Domain.Entities;
using ToDoApi.Services.Tasks.Domain.Repository;
using ToDoApi.Services.Tasks.Infrastructure.Data;

namespace ToDoApi.Services.Tasks.Infrastructure.Repository;

public class ToDoTaskRepository : ITodoTaskRepository
{
    private readonly ToDoTaskDataContext _context;

    public ToDoTaskRepository(ToDoTaskDataContext context)
    {
        _context = context;
    }

    public DbContext DatabaseContext => _context;

    public IQueryable<ToDoTask> Query => _context.Tasks;

    public Task Create(ToDoTask item)
    {
        return Task.FromResult(_context.Tasks.Add(item));
    }

    public async Task<ToDoTask?> GetById(Guid id)
    {
        return await Query.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<ToDoTask>> GetAll()
    {
        return await Query.ToListAsync();
    }

    public Task Delete(ToDoTask item)
    {
        return Task.FromResult(_context.Tasks.Remove(item));
    }

    public Task Update(ToDoTask item)
    {
        return Task.FromResult(_context.Entry(item).State = EntityState.Modified);
    }
}