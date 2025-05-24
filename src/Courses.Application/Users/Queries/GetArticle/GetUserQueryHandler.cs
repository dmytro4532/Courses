using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Users.Dto;
using Courses.Domain.Users;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Users.Queries.GetArticle;

internal sealed class GetUserQueryHandler : IQueryHandler<GetUserQuery, Result<UserResponse>>
{
    private readonly IUserRepository _userRepository;
    private readonly Mapper<User, UserResponse> _mapper;

    public GetUserQueryHandler(IUserRepository userRepository, Mapper<User, UserResponse> mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
    {
        var user = await _userRepository.GetByIdAsync(request.UserId, cancellationToken);

        if (user is null)
        {
            return new NotFoundError("User.NotFound", "User not found.");
        }

        return _mapper.Map(user);
    }
}
