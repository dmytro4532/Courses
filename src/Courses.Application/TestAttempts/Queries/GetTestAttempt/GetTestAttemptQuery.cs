using Courses.Application.TestAttempts.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.TestAttempts.Queries.GetTestAttempt;

public record GetTestAttemptQuery(Guid TestAttemptId) : IRequest<Result<TestAttemptResponse>>; 