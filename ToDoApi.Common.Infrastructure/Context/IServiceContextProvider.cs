using ToDoApi.Common.Core.Context;

namespace ToDoApi.Common.Infrastructure.Context;

public interface IServiceContextProvider
{
    public IIdentityContext ProvideIdentityContext();
}