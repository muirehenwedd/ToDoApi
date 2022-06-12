using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using ToDoApi.Services.Identity.Domain.Entities;
using ToDoApi.Services.Identity.Domain.Repository;
using ToDoApi.Services.Identity.Infrastructure.Data;

namespace ToDoApi.Services.Identity.Infrastructure.Repository;

internal class UserRepository : IUserRepository
{
    private readonly IdentityDataContext _context;

    public UserRepository(IdentityDataContext context)
    {
        _context = context;
    }

    public DbContext DatabaseContext => _context;

    public IQueryable<User> Query => _context.Users
        .Include(u => u.RefreshTokens)
        .AsSplitQuery();

    public Task Create(User item)
    {
        return Task.FromResult(_context.Users.Add(item));
    }

    public async Task<User?> GetById(Guid id)
    {
        return await Query.FirstOrDefaultAsync(u => u.Id == id);
    }

    public async Task<IEnumerable<User>> GetAll()
    {
        return await Query.ToListAsync();
    }

    public Task Delete(User item)
    {
        return Task.FromResult(_context.Users.Remove(item));
    }

    public Task Update(User item)
    {
        return Task.FromResult(_context.Entry(item).State = EntityState.Modified);
    }
}