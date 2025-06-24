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
            return new Error("User.WrongEmailOrPassword", "Неправильна електронна пошта або пароль.");
        }

        if (!identityUser.EmailConfirmed)
        {
            return new Error("User.EmailNotConfirmed", "Підтвердьте свою електронну пошту, щоб увійти.");
        }

        var loginResult = await _identityService.LoginAsync(
            identityUser,
            password: request.Password);

        if (loginResult.IsFailure)
        {
            return new Error("User.WrongEmailOrPassword", "Неправильна електронна пошта або пароль.");
        }

        var token = _tokenService.GenerateAccessToken(identityUser);

        return new TokenResponse(token);
    }
}
