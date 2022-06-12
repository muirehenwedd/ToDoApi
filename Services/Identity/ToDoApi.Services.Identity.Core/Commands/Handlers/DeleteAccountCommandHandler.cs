using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Services.Identity.Core.Exceptions;
using ToDoApi.Services.Identity.Domain.Repository;

namespace ToDoApi.Services.Identity.Core.Commands.Handlers;

public class DeleteAccountCommandHandler : ICommandHandler<DeleteAccountCommand>
{
    private readonly IIdentityContext _identityContext;
    private readonly IUserRepository _userRepository;

    public DeleteAccountCommandHandler(
        IIdentityContext serviceContext,
        IUserRepository userRepository
    )
    {
        _identityContext = serviceContext;
        _userRepository = userRepository;
    }

    public async Task HandleAsync(DeleteAccountCommand command)
    {
        var userId = _identityContext.UserId;
        var user = await _userRepository.GetById(userId);

        if (user is null)
            throw new UserIsNotFoundException();

        await _userRepository.CommitAsync(async () => { await _userRepository.Delete(user); });
    }
}