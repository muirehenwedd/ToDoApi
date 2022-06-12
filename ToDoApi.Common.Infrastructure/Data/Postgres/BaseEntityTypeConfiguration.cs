using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ToDoApi.Common.Infrastructure.Data.Postgres;

public abstract class BaseEntityTypeConfiguration<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity : class
{
    protected BaseEntityTypeConfiguration(string schema)
    {
        _schema = schema;
    }

    private string _schema { get; }

    protected abstract string Table { get; }
    protected abstract Expression<Func<TEntity, object>> PrimaryKey { get; }

    void IEntityTypeConfiguration<TEntity>.Configure(EntityTypeBuilder<TEntity> builder)
    {
        Configure(builder);

        builder
            .ToTable(Table, _schema)
            .HasKey(PrimaryKey);
    }

    protected abstract void Configure(EntityTypeBuilder<TEntity> builder);
}