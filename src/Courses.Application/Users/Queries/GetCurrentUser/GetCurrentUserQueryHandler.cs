using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Abstractions.Services;
using Courses.Application.Users.Dto;
using Courses.Application.Users.Queries.GetCurrentUser;
using Courses.Domain.Users;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Users.Queries.GetArticle;

internal sealed class GetCurrentUserQueryHandler : IQueryHandler<GetCurrentUserQuery, Result<UserResponse>>
{
    private readonly IUserContext _userContext;
    private readonly IUserRepository _userRepository;
    private readonly Mapper<User, UserResponse> _mapper;

    public GetCurrentUserQueryHandler( IUserRepository userRepository, Mapper<User, UserResponse> mapper, IUserContext userContext)
    {
        _userRepository = userRepository;
        _mapper = mapper;
        _userContext = userContext;
    }

    public async Task<Result<UserResponse>> Handle(GetCurrentUserQuery request, CancellationToken cancellationToken)
    {
        var userId = _userContext.UserId;
        var user = await _userRepository.GetByIdAsync(userId, cancellationToken);

        if (user is null)
        {
            return new NotFoundError("User.NotFound", "User not found.");
        }

        return _mapper.Map(user);
    }
}
