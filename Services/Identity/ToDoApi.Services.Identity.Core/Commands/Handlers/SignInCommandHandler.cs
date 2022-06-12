using System.Threading.Tasks;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Services.Identity.Core.Commands.Responses;
using ToDoApi.Services.Identity.Core.Exceptions;
using ToDoApi.Services.Identity.Core.Services;
using ToDoApi.Services.Identity.Domain.Repository;

namespace ToDoApi.Services.Identity.Core.Commands.Handlers;

public class SignInCommandHandler : IResultCommandHandler<SignInCommand, SignInCommandResponse>
{
    private readonly ICryptoService _cryptoService;
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public SignInCommandHandler(
        ICryptoService cryptoService,
        IUserRepository userRepository,
        IJwtService jwtService
    )
    {
        _cryptoService = cryptoService;
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<SignInCommandResponse> HandleAsync(SignInCommand command)
    {
        var user = await _userRepository
            .FirstOrDefault(u => u.Login.Equals(command.Login));

        if (user is null)
            throw new UserIsNotFoundException();

        if (!_cryptoService.ValidateUserPassword(user, command.Password))
            throw new InvalidPasswordException(command.Login);

        var refreshToken = _cryptoService.GenerateRefreshToken();
        var accessTokenData = await _jwtService.GenerateAccessToken(user);

        await _userRepository.CommitAsync(async () =>
        {
            user.AddRefreshToken(refreshToken);
            await _userRepository.Update(user);
        });

        return new SignInCommandResponse
        {
            UserId = user.Id,
            Token = accessTokenData.AccessToken,
            TokenExpiresAt = accessTokenData.ExpiresAt,
            RefreshToken = refreshToken
        };
    }
}