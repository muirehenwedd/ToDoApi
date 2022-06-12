using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApi.Common.Infrastructure.Data.Postgres;
using ToDoApi.Services.Identity.Domain.Entities;

namespace ToDoApi.Services.Identity.Infrastructure.Data.Configurators;

public class UserEntityConfiguration : BaseEntityTypeConfiguration<User>
{
    public UserEntityConfiguration(string schema) : base(schema)
    {
    }

    protected override string Table => "user";
    protected override Expression<Func<User, object>> PrimaryKey => user => user.Id;

    protected override void Configure(EntityTypeBuilder<User> builder)
    {
        builder
            .PropertyColumn(e => e.Id)
            .HasColumnType("uuid")
            .IsRequired();

        builder.PropertyColumn(e => e.Login);
        builder.PropertyColumn(e => e.PasswordHash);
        builder.PropertyColumn(e => e.PasswordSalt);
        builder.PropertyColumn(e => e.CreationTimestamp);

        builder
            .HasMany(e => e.RefreshTokens)
            .WithOne(e => e.User);
    }
}