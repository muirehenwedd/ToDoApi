using System;
using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ToDoApi.Common.Infrastructure.Data.Postgres;
using ToDoApi.Services.Tasks.Domain.Entities;

namespace ToDoApi.Services.Tasks.Infrastructure.Data.Configuration;

public class ToDoTaskEntityConfiguration : BaseEntityTypeConfiguration<ToDoTask>
{
    public ToDoTaskEntityConfiguration(string schema) : base(schema)
    {
    }

    protected override string Table => "task";
    protected override Expression<Func<ToDoTask, object>> PrimaryKey => user => user.Id;

    protected override void Configure(EntityTypeBuilder<ToDoTask> builder)
    {
        builder
            .PropertyColumn(e => e.Id)
            .HasColumnType("uuid")
            .IsRequired();

        builder.PropertyColumn(e => e.User);
        builder.PropertyColumn(e => e.Title);
        builder.PropertyColumn(e => e.Description);
        builder.PropertyColumn(e => e.CreationTimestamp);
        builder.PropertyColumn(e => e.IsDone);
    }
}