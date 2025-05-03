using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Users.Dto;
using Courses.Domain.User;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Users.Commands.CreateUser;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IPasswordHasher _passwordHasher;
    private readonly Mapper<User, UserResponse> _mapper;

    public RegisterUserCommandHandler(IUserRepository userRepository, IUnitOfWork unitOfWork, IPasswordHasher passwordHasher, Mapper<User, UserResponse> mapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _passwordHasher = passwordHasher;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userByEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if(userByEmail is not null)
        {
            return new Error("User.EmailAlreadyExists", "Email already used.");
        }

        var passwordHash = _passwordHasher.Hash(request.Password);

        var user = User.Create(
            Guid.NewGuid(),
            Username.Create(request.Username),
            Email.Create(request.Email),
            passwordHash,
            Role.User);

        await _userRepository.AddAsync(user, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map(user);
    }
}
