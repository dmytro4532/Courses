using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.Common.Exceptions;
using Courses.Application.Users.Commands.ConfirmEmail;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Users.Commands.CreateUser;

internal sealed class ConfirmEmailHandler : IRequestHandler<ConfirmEmailCommand, Result>
{
    private readonly IUserRepository _userRepository;
    private readonly IIdentityService _identityService;

    public ConfirmEmailHandler(IUserRepository userRepository,
        IIdentityService identityService)
    {
        _userRepository = userRepository;
        _identityService = identityService;
    }

    public async Task<Result> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
    {

        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return new NotFoundError("User.NotFound", "User not found.");
        }

        var identityUser = await _identityService.GetByEmailAsync(user.Email)
            ?? throw new InconsistentDataException($"Identity with email '{user.Email}' not found");

        if (identityUser.EmailConfirmed)
        {
            return new NotFoundError("User.EmailAlreadyConfirmed", "Email already confirmed.");
        }

        return await _identityService.ConfirmEmailAsync(identityUser, request.Token);
    }
}
