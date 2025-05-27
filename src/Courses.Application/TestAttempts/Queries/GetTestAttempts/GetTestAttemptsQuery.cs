using Courses.Application.Common.Models;
using Courses.Application.TestAttempts.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.TestAttempts.Queries.GetTestAttempts;

public record GetTestAttemptsQuery(
    int PageIndex,
    int PageSize,
    string OrderBy,
    string OrderDirection) : IRequest<Result<PagedList<TestAttemptResponse>>>; 