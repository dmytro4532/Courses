using Courses.Application.Abstractions.Mapping;
using Courses.Application.Tests.Dto;
using Courses.Domain.Tests;

namespace Courses.Application.Tests.Mapping;

public sealed class TestResponseMapper : Mapper<Test, TestResponse>
{
    public override TestResponse Map(Test source)
    {
        return new TestResponse(
            source.Id,
            source.Title.Value,
            source.CreatedAt,
            source.UpdatedAt);
    }
} 