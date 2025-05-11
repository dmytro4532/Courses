using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Tests.Dto;
using Courses.Domain.Tests;
using Shared.Results;

namespace Courses.Application.Tests.Commands.CreateTest;

internal sealed class CreateTestCommandHandler : ICommandHandler<CreateTestCommand, Result<TestResponse>>
{
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Test, TestResponse> _mapper;

    public CreateTestCommandHandler(
        ITestRepository testRepository,
        IUnitOfWork unitOfWork,
        Mapper<Test, TestResponse> mapper)
    {
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TestResponse>> Handle(CreateTestCommand request, CancellationToken cancellationToken)
    {
        var test = Test.Create(Guid.NewGuid(), Title.Create(request.Title), request.TopicId);

        await _testRepository.AddAsync(test, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map(test));
    }
} 