using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Users.Dto;
using Courses.Application.Users.Identity;
using Courses.Domain.User;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Users.Commands.CreateUser;

internal sealed class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IIdentityService _identityService;
    private readonly Mapper<User, UserResponse> _mapper;
    private readonly Mapper<User, ApplicationUser> _applicationUserMapper;

    public RegisterUserCommandHandler(IUserRepository userRepository,
        IUnitOfWork unitOfWork,
        IIdentityService identityService,
        Mapper<User, UserResponse> mapper,
        Mapper<User, ApplicationUser> applicationUserMapper)
    {
        _userRepository = userRepository;
        _unitOfWork = unitOfWork;
        _identityService = identityService;
        _mapper = mapper;
        _applicationUserMapper = applicationUserMapper;
    }

    public async Task<Result<UserResponse>> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var userByEmail = await _userRepository.GetByEmailAsync(request.Email, cancellationToken);

        if (userByEmail is not null)
        {
            return new Error("User.EmailAlreadyExists", "Email already used.");
        }

        var user = User.Create(
            Guid.NewGuid(),
            Username.Create(request.Username),
            Email.Create(request.Email));

        var applicaitonUser = _applicationUserMapper.Map(user);

        applicaitonUser.Role = Role.User.ToString();

        // TODO: remove this when we have a real email confirmation
        applicaitonUser.EmailConfirmed = true;

        await _userRepository.AddAsync(user, cancellationToken);

        await _identityService.CreateAsync(applicaitonUser, request.Password);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map(user);
    }
}
