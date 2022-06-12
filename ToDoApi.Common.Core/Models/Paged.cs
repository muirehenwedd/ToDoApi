using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace ToDoApi.Common.Core.Models;

public class Paged<T> : List<T>
{
    public int CurrentPage { get; private set; }
    public int TotalPages { get; private set; }
    public int PageSize { get; private set; }
    public int Total { get; private set; }

    public Paged(IEnumerable<T> items, int total, int currentPage, int pageSize)
    {
        Total = total;
        PageSize = pageSize;
        CurrentPage = currentPage;
        TotalPages = (int) Math.Ceiling(total / (double) pageSize);
        AddRange(items);
    }

    public static Paged<T> ToPagedList(IEnumerable<T> items, int page, int top)
    {
        var total = items.Count();
        var pagedItems = items.Skip(page * top).Take(top).ToList();
        return new Paged<T>(pagedItems, total, page, top);
    }

    public static async Task<Paged<T>> ToPagedListAsync(IQueryable<T> items, int page, int top)
    {
        var total = items.Count();
        var pagedItems = await items.Skip(page * top).Take(top).ToListAsync();
        return new Paged<T>(pagedItems, total, page, top);
    }
}