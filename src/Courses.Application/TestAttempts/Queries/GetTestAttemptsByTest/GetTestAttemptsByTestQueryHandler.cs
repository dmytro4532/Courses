using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.Common.Models;
using Courses.Application.TestAttempts.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.TestAttempts.Queries.GetTestAttemptsByTest;

internal sealed class GetTestAttemptsByTestQueryHandler : IRequestHandler<GetTestAttemptsByTestQuery, Result<PagedList<TestAttemptResponse>>>
{
    private readonly ITestAttemptRepository _testAttemptRepository;
    private readonly IUserContext _userContext;

    public GetTestAttemptsByTestQueryHandler(ITestAttemptRepository testAttemptRepository, IUserContext userContext)
        => (_testAttemptRepository, _userContext) = (testAttemptRepository, userContext);

    public async Task<Result<PagedList<TestAttemptResponse>>> Handle(GetTestAttemptsByTestQuery request, CancellationToken cancellationToken)
    {
        var testAttempts = await _testAttemptRepository.GetByTestIdAndUserIdAsync(
            request.TestId,
            _userContext.UserId,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.OrderDirection,
            cancellationToken);

        var totalCount = await _testAttemptRepository.CountByTestIdAndUserIdAsync(
            request.TestId,
            _userContext.UserId,
            cancellationToken);

        var testAttemptResponses = testAttempts.Select(ta => new TestAttemptResponse(
            ta.Id,
            ta.TestId,
            ta.UserId,
            ta.CreatedAt,
            ta.CompletedAt,
            ta.Score));

        return Result.Success(new PagedList<TestAttemptResponse>(
            testAttemptResponses,
            request.PageIndex,
            request.PageSize,
            totalCount));
    }
} 