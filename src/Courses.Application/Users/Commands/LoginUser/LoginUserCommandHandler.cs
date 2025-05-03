using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Users.Dto;
using Courses.Domain.User;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Users.Commands.LoginUser;

internal sealed class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IPasswordHasher _passwordHasher;
    private readonly ITokenService _tokenService;
    private readonly Mapper<User, UserResponse> _mapper;

    public LoginUserCommandHandler(IUserRepository userRepository, IPasswordHasher passwordHasher, ITokenService tokenService, Mapper<User, UserResponse> mapper)
    {
        _userRepository = userRepository;
        _passwordHasher = passwordHasher;
        _tokenService = tokenService;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(LoginUserCommand request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (user is null || _passwordHasher.Verify(request.Password, user.PasswordHash))
        {
            return new Error("User.WrongEmailOrPassword", "Wrong email or password.");
        }

        if (!user.EmailConfirmed)
        {
            return new Error("User.EmailNotConfirmed", "Confirm your email to log in.");
        }

        var token = _tokenService.GenerateAccessToken(user);

        return _mapper.Map(user);
    }
}
