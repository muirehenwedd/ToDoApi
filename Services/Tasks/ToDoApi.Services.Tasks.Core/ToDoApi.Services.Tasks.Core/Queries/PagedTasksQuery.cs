using ToDoApi.Common.Core.Attributes;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Common.Infrastructure.Validation.Attributes;

namespace ToDoApi.Services.Tasks.Core.Queries;

public class PagedTasksQuery : IQuery
{
    
    [Required]
    [PayloadProperty("top")]
    [InRange(MinValue = 1)]
    public int Top { get; set; }

    [Required]
    [PayloadProperty("page")]
    [InRange(MinValue = 0)]
    public int Page { get; set; }
}