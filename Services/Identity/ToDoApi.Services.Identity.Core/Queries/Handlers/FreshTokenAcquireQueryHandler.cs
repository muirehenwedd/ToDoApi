using System.Linq;
using System.Threading.Tasks;
using ToDoApi.Common.Core.CQRS.Queries;
using ToDoApi.Services.Identity.Core.Exceptions;
using ToDoApi.Services.Identity.Core.Queries.Responses;
using ToDoApi.Services.Identity.Core.Services;
using ToDoApi.Services.Identity.Domain.Repository;

namespace ToDoApi.Services.Identity.Core.Queries.Handlers;

public class FreshTokenAcquireQueryHandler : IQueryHandler<FreshTokenAcquireQuery, FreshTokenAcquireQueryResponse>
{
    private readonly IJwtService _jwtService;
    private readonly IUserRepository _userRepository;

    public FreshTokenAcquireQueryHandler(IUserRepository userRepository, IJwtService jwtService)
    {
        _userRepository = userRepository;
        _jwtService = jwtService;
    }

    public async Task<FreshTokenAcquireQueryResponse> HandleAsync(FreshTokenAcquireQuery query)
    {
        var users = await _userRepository.GetAll();
        var user = users.FirstOrDefault(u => u.Id.Equals(query.UserId));

        var refreshToken = user?.RefreshTokens.FirstOrDefault(r => r.Token.Equals(query.RefreshToken));

        if (refreshToken is null)
            throw new InvalidRenewSessionTokenException();

        if (refreshToken.IsRevoked)
        {
            await _userRepository.CommitAsync(async () =>
            {
                user.DeleteRefreshToken(refreshToken);
                await _userRepository.Update(user);
            });

            throw new RevokedTokenException();
        }

        var accessTokenData = await _jwtService.GenerateAccessToken(user);

        return new FreshTokenAcquireQueryResponse
        {
            Token = accessTokenData.AccessToken,
            TokenExpiresAt = accessTokenData.ExpiresAt
        };
    }
}