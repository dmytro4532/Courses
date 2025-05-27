using Courses.Application.Abstractions.Messaging;
using Shared.Results;

namespace Courses.Application.TestAttempts.Commands.CompleteTestAttempt;

public class CompleteTestAttemptCommand : ICommand<Result>
{
    public required Guid TestAttemptId { get; init; }
} 