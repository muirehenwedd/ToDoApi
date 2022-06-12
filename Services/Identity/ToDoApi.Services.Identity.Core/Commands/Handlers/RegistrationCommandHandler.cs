using System;
using System.Threading.Tasks;
using ToDoApi.Common.Core.CQRS.Commands;
using ToDoApi.Services.Identity.Core.Commands.Responses;
using ToDoApi.Services.Identity.Core.Exceptions;
using ToDoApi.Services.Identity.Core.Services;
using ToDoApi.Services.Identity.Domain.Entities;
using ToDoApi.Services.Identity.Domain.Repository;

namespace ToDoApi.Services.Identity.Core.Commands.Handlers;

public sealed class RegistrationCommandHandler : IResultCommandHandler<RegistrationCommand, RegistrationCommandResponse>
{
    private readonly ICryptoService _cryptoService;
    private readonly IUserRepository _userRepository;

    public RegistrationCommandHandler(ICryptoService cryptoService, IUserRepository userRepository)
    {
        _cryptoService = cryptoService;
        _userRepository = userRepository;
    }

    public async Task<RegistrationCommandResponse> HandleAsync(RegistrationCommand command)
    {
        if (await _userRepository.Any(u => u.Login == command.Login))
            throw new LoginAlreadyRegisteredException(command.Login);

        var passwordSalt = _cryptoService.GenerateSalt();
        var passwordHash = _cryptoService.EncryptPassword(command.Password, passwordSalt);

        var userId = Guid.NewGuid();

        var user = new User
        {
            Id = userId,
            Login = command.Login,
            PasswordHash = passwordHash,
            PasswordSalt = passwordSalt,
            CreationTimestamp = DateTime.UtcNow
        };

        await _userRepository.CommitAsync(async () => { await _userRepository.Create(user); });

        return new RegistrationCommandResponse
        {
            UserId = userId
        };
    }
}