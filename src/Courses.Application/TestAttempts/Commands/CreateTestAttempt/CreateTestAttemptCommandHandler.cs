using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using Courses.Application.TestAttempts.Dto;
using Courses.Domain.TestAttempts;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.TestAttempts.Commands.CreateTestAttempt;

internal sealed class CreateTestAttemptCommandHandler : IRequestHandler<CreateTestAttemptCommand, Result<TestAttemptResponse>>
{
    private readonly ITestAttemptRepository _testAttemptRepository;
    private readonly ITestRepository _testRepository;
    private readonly IUserContext _userContext;
    private readonly IUnitOfWork _unitOfWork;

    public CreateTestAttemptCommandHandler(
        ITestAttemptRepository testAttemptRepository,
        ITestRepository testRepository,
        IUserContext userContext,
        IUnitOfWork unitOfWork)
    {
        _testAttemptRepository = testAttemptRepository;
        _testRepository = testRepository;
        _userContext = userContext;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result<TestAttemptResponse>> Handle(CreateTestAttemptCommand request, CancellationToken cancellationToken)
    {
        var test = await _testRepository.GetByIdAsync(request.TestId, cancellationToken);

        if (test is null)
        {
            return Result.Failure<TestAttemptResponse>(new NotFoundError("Test not found.", "Test not found."));
        }

        var testAttempt = TestAttempt.Create(Guid.NewGuid(), request.TestId, _userContext.UserId);

        await _testAttemptRepository.AddAsync(testAttempt, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(new TestAttemptResponse(
            testAttempt.Id,
            testAttempt.TestId,
            testAttempt.UserId,
            testAttempt.CreatedAt,
            testAttempt.CompletedAt,
            testAttempt.Score));
    }
} 