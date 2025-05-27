using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.Common.Models;
using Courses.Application.TestAttempts.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.TestAttempts.Queries.GetTestAttempts;

internal sealed class GetTestAttemptsQueryHandler : IRequestHandler<GetTestAttemptsQuery, Result<PagedList<TestAttemptResponse>>>
{
    private readonly ITestAttemptRepository _testAttemptRepository;
    private readonly IUserContext _userContext;

    public GetTestAttemptsQueryHandler(
        ITestAttemptRepository testAttemptRepository,
        IUserContext userContext)
    {
        _testAttemptRepository = testAttemptRepository;
        _userContext = userContext;
    }

    public async Task<Result<PagedList<TestAttemptResponse>>> Handle(GetTestAttemptsQuery request, CancellationToken cancellationToken)
    {
        var testAttempts = await _testAttemptRepository.GetByUserIdAsync(
            _userContext.UserId,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.OrderDirection,
            cancellationToken);

        var totalCount = await _testAttemptRepository.CountByUserIdAsync(_userContext.UserId, cancellationToken);

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