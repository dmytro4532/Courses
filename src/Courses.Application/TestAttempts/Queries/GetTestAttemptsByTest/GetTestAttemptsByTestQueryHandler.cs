using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Common.Models;
using Courses.Application.TestAttempts.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.TestAttempts.Queries.GetTestAttemptsByTest;

internal sealed class GetTestAttemptsByTestQueryHandler : IRequestHandler<GetTestAttemptsByTestQuery, Result<PagedList<TestAttemptResponse>>>
{
    private readonly ITestAttemptRepository _testAttemptRepository;

    public GetTestAttemptsByTestQueryHandler(ITestAttemptRepository testAttemptRepository)
        => _testAttemptRepository = testAttemptRepository;

    public async Task<Result<PagedList<TestAttemptResponse>>> Handle(GetTestAttemptsByTestQuery request, CancellationToken cancellationToken)
    {
        var testAttempts = await _testAttemptRepository.GetByTestIdAsync(
            request.TestId,
            request.PageIndex,
            request.PageSize,
            request.OrderBy,
            request.OrderDirection,
            cancellationToken);

        var totalCount = await _testAttemptRepository.CountByTestIdAsync(request.TestId, cancellationToken);

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