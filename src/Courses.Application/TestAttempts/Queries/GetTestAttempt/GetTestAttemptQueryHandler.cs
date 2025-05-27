using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.TestAttempts.Dto;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.TestAttempts.Queries.GetTestAttempt;

internal sealed class GetTestAttemptQueryHandler : IRequestHandler<GetTestAttemptQuery, Result<TestAttemptResponse>>
{
    private readonly ITestAttemptRepository _testAttemptRepository;

    public GetTestAttemptQueryHandler(ITestAttemptRepository testAttemptRepository)
        => _testAttemptRepository = testAttemptRepository;

    public async Task<Result<TestAttemptResponse>> Handle(GetTestAttemptQuery request, CancellationToken cancellationToken)
    {
        var testAttempt = await _testAttemptRepository.GetByIdAsync(request.TestAttemptId, cancellationToken);

        if (testAttempt is null)
        {
            return Result.Failure<TestAttemptResponse>(new NotFoundError("TestAttempt.NotFound", "Test attempt not found."));
        }

        return Result.Success(new TestAttemptResponse(
            testAttempt.Id,
            testAttempt.TestId,
            testAttempt.UserId,
            testAttempt.CreatedAt,
            testAttempt.CompletedAt,
            testAttempt.Score));
    }
} 