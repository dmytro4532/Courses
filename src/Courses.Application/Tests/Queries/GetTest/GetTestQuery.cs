using Courses.Application.Abstractions.Messaging;
using Courses.Application.Tests.Dto;
using Shared.Results;

namespace Courses.Application.Tests.Queries.GetTest;

public record GetTestQuery(Guid Id) : IQuery<Result<TestResponse>>; 