using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApi.Common.Infrastructure.Data.Postgres;
using ToDoApi.Services.Identity.Domain.Entities;

namespace ToDoApi.Services.Identity.Infrastructure.Data.Configurators;

public class RefreshTokenEntityConfiguration : BaseEntityTypeConfiguration<RefreshToken>
{
    public RefreshTokenEntityConfiguration(string schema) : base(schema)
    {
    }

    protected override string Table => "refresh_token";
    protected override Expression<Func<RefreshToken, object>> PrimaryKey => token => token.Id;

    protected override void Configure(EntityTypeBuilder<RefreshToken> builder)
    {
        builder
            .PropertyColumn(e => e.Id)
            .HasColumnType("uuid")
            .IsRequired();

        builder.PropertyColumn(e => e.Token);
        builder.PropertyColumn(e => e.CreatedTimestamp);
        builder.PropertyColumn(e => e.RevokedTimestamp);

        builder
            .HasOne(e => e.User)
            .WithMany(e => e.RefreshTokens)
            .HasForeignKey("user")
            .OnDelete(DeleteBehavior.Cascade);
    }
}