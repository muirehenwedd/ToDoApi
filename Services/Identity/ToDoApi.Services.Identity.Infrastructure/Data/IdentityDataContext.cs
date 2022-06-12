using Microsoft.EntityFrameworkCore;
using ToDoApi.Common.Infrastructure.Data.Postgres;
using ToDoApi.Services.Identity.Domain.Entities;
using ToDoApi.Services.Identity.Infrastructure.Data.Configurators;

namespace ToDoApi.Services.Identity.Infrastructure.Data;

public class IdentityDataContext : ServiceDbContext
{
    public IdentityDataContext(DbContextOptions<IdentityDataContext> options) : base(options)
    {
    }

    public override string Schema => "identity";

    public DbSet<User> Users { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new UserEntityConfiguration(Schema));
        modelBuilder.ApplyConfiguration(new RefreshTokenEntityConfiguration(Schema));

        base.OnModelCreating(modelBuilder);
    }
}