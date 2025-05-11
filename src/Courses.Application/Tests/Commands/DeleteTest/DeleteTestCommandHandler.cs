using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Messaging;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Tests.Commands.DeleteTest;

internal sealed class DeleteTestCommandHandler : ICommandHandler<DeleteTestCommand, Result>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTestCommandHandler(
        ITestRepository testRepository,
        IUnitOfWork unitOfWork)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTestCommand request, CancellationToken cancellationToken)
    {
        var test = await _testRepository.GetByIdAsync(request.Id, cancellationToken);

        if (test is null)
        {
            return Result.Failure(new NotFoundError("Test.NotFound", "Test not found"));
        }

        _testRepository.Remove(test);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
} 