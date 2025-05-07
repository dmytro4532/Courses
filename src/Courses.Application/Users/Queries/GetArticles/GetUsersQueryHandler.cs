using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Users.Dto;
using Courses.Domain.User;
using Shared.Results;

namespace Courses.Application.Users.Queries.GetArticles;

internal sealed class GetUsersQueryHandler : IQueryHandler<GetUsersQuery, Result<PagedList<UserResponse>>>
{
    private readonly IUserRepository _userRepository;
    private readonly Mapper<User, UserResponse> _mapper;

    public GetUsersQueryHandler(IUserRepository userRepository, Mapper<User, UserResponse> mapper)
    {
        _userRepository = userRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<UserResponse>>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
    {
        var articles = _mapper.Map(
            await _userRepository.Get(
                request.PageIndex,
                request.PageSize,
                request.OrderBy,
                request.OrderDirection,
                cancellationToken));

        var totalCount = await _userRepository.CountAsync(cancellationToken);

        return new PagedList<UserResponse>(articles, request.PageIndex, request.PageSize, totalCount);
    }
}
