using Courses.Application.Abstractions.Services;
using Courses.Application.Users.Dto;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Users.Commands.LoginUser;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<TokenResponse>>
{
    private readonly IIdentityService _identityService;
    private readonly ITokenService _tokenService;

    public LoginUserCommandHandler(IIdentityService identityService, ITokenService tokenService)
    {
        _identityService = identityService;
        _tokenService = tokenService;
    }

    public async Task<Result<TokenResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var identityUser = await _identityService.GetByEmailAsync(request.Email);

        if (identityUser is null)
        {
            return new Error("User.WrongEmailOrPassword", "Wrong email or password.");
        }

        if (!identityUser.EmailConfirmed)
        {
            return new Error("User.EmailNotConfirmed", "Confirm your email to log in.");
        }

        var loginResult = await _identityService.LoginAsync(
            identityUser,
            password: request.Password);

        if (loginResult.IsFailure)
        {
            return new Error("User.WrongEmailOrPassword", "Wrong email or password.");
        }

        var token = _tokenService.GenerateAccessToken(identityUser);

        return new TokenResponse(token);
    }
}
