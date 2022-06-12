using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Common.Infrastructure.Data.Postgres;

public abstract class ServiceDbContext : DbContext
{
    protected ServiceDbContext(DbContextOptions options) : base(options)
    {
    }

    public abstract string Schema { get; }
}