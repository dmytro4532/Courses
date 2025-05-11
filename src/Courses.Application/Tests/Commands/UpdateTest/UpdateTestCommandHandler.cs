using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Tests.Dto;
using Courses.Domain.Tests;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Tests.Commands.UpdateTest;

internal sealed class UpdateTestCommandHandler : ICommandHandler<UpdateTestCommand, Result<TestResponse>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Test, TestResponse> _mapper;

    public UpdateTestCommandHandler(
        ITestRepository testRepository,
        IUnitOfWork unitOfWork,
        Mapper<Test, TestResponse> mapper)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TestResponse>> Handle(UpdateTestCommand request, CancellationToken cancellationToken)
    {
        var test = await _testRepository.GetByIdAsync(request.Id, cancellationToken);

        if (test is null)
        {
            return Result.Failure<TestResponse>(new NotFoundError("Test.NotFound", "Test not found"));
        }

        test.Update(Title.Create(request.Title));

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map(test));
    }
} 