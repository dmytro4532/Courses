using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Tests.Dto;
using Courses.Domain.Tests;
using Shared.Results;

namespace Courses.Application.Tests.Queries.GetTests;

internal sealed class GetTestsQueryHandler : IQueryHandler<GetTestsQuery, Result<PagedList<TestResponse>>>
{
    private readonly ITestRepository _testRepository;
    private readonly Mapper<Test, TestResponse> _mapper;

    public GetTestsQueryHandler(ITestRepository testRepository, Mapper<Test, TestResponse> mapper)
    {
        _testRepository = testRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<TestResponse>>> Handle(GetTestsQuery request, CancellationToken cancellationToken)
    {
        var tests = _mapper.Map(
            await _testRepository.Get(
                request.PageIndex,
                request.PageSize,
                request.OrderBy,
                request.OrderDirection,
                cancellationToken));

        var totalCount = await _testRepository.CountAsync(cancellationToken);

        return new PagedList<TestResponse>(tests, request.PageIndex, request.PageSize, totalCount);
    }
} 