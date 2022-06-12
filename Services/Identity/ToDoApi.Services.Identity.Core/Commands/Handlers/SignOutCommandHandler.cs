using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Common.Core.Context;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Services.Identity.Core.Exceptions;
using ToDoApi.Services.Identity.Domain.Repository;

namespace ToDoApi.Services.Identity.Core.Commands.Handlers;

public sealed class SignOutCommandHandler : ICommandHandler<SignOutCommand>
{
    private readonly IIdentityContext _identityContext;
    private readonly IUserRepository _userRepository;

    public SignOutCommandHandler(IIdentityContext serviceContext, IUserRepository userRepository)
    {
        _identityContext = serviceContext;
        _userRepository = userRepository;
    }

    public async Task HandleAsync(SignOutCommand command)
    {
        var userId = _identityContext.UserId;
        var user = await _userRepository.GetById(userId);

        var refreshToken = user?.RefreshTokens.FirstOrDefault(r =>
            r.Token.Equals(command.RefreshToken));

        if (refreshToken is null)
            throw new InvalidLogoutTokenException();

        if (refreshToken.IsRevoked)
            throw new SessionAlreadyEndedException();

        await _userRepository.CommitAsync(async () =>
        {
            user.RevokeToken(refreshToken);
            await _userRepository.Update(user);
        });
    }
}