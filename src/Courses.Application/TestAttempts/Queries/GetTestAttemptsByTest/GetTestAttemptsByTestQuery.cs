using Courses.Application.Common.Models;
using Courses.Application.TestAttempts.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.TestAttempts.Queries.GetTestAttemptsByTest;

public record GetTestAttemptsByTestQuery(
    Guid TestId,
    int PageIndex,
    int PageSize,
    string OrderBy,
    string OrderDirection) : IRequest<Result<PagedList<TestAttemptResponse>>>; 