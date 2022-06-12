using Microsoft.EntityFrameworkCore;
using ToDoApi.Common.Infrastructure.Data.Postgres;
using ToDoApi.Services.Tasks.Domain.Entities;
using ToDoApi.Services.Tasks.Infrastructure.Data.Configuration;

namespace ToDoApi.Services.Tasks.Infrastructure.Data;

public class ToDoTaskDataContext : ServiceDbContext
{
    public ToDoTaskDataContext(DbContextOptions<ToDoTaskDataContext> options) : base(options)
    {
    }

    public override string Schema => "task";

    public DbSet<ToDoTask> Tasks { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new ToDoTaskEntityConfiguration(Schema));

        base.OnModelCreating(modelBuilder);
    }
}