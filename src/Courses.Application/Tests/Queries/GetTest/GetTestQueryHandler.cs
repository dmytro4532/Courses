using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Tests.Dto;
using Courses.Domain.Tests;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Tests.Queries.GetTest;

internal sealed class GetTestQueryHandler : IQueryHandler<GetTestQuery, Result<TestResponse>>
{
    private readonly ITestRepository _testRepository;
    private readonly Mapper<Test, TestResponse> _mapper;

    public GetTestQueryHandler(ITestRepository testRepository, Mapper<Test, TestResponse> mapper)
    {
        _testRepository = testRepository;
        _mapper = mapper;
    }

    public async Task<Result<TestResponse>> Handle(GetTestQuery request, CancellationToken cancellationToken)
    {
        var test = await _testRepository.GetByIdAsync(request.Id, cancellationToken);

        if (test is null)
        {
            return new NotFoundError("Test.NotFound", "The test was not found.");
        }

        return _mapper.Map(test);
    }
} 