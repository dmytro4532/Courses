using Courses.Application.Abstractions.Messaging;
using Courses.Application.TestAttempts.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.TestAttempts.Commands.CreateTestAttempt;

public class CreateTestAttemptCommand : IRequest<Result<TestAttemptResponse>>
{
    public required Guid TestId { get; init; }
} 