using System;
using Microsoft.AspNetCore.Http;
using ToDoApi.Common.Auth;
using ToDoApi.Common.Auth.Contracts;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Infrastructure.Exceptions;

namespace ToDoApi.Common.Infrastructure.Context.Internals;

internal class ServiceContextProvider : IServiceContextProvider
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ServiceContextProvider(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public IIdentityContext ProvideIdentityContext()
    {
        var context = _httpContextAccessor.HttpContext;

        if (context is null)
            throw new NullReferenceException("HttpContext is no available in scope.");

        var tokenPayloadItem = context.Items[IdentityItems.TokenPayload];

        if (tokenPayloadItem is not TokenPayload tokenPayload)
            throw new IdentityContextUnavailableException();

        var identity = new IdentityContext
        {
            UserId = tokenPayload.UserId
        };

        return identity;
    }
}