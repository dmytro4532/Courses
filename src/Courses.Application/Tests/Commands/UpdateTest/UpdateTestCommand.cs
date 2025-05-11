using Courses.Application.Abstractions.Messaging;
using Courses.Application.Tests.Dto;
using Shared.Results;

namespace Courses.Application.Tests.Commands.UpdateTest;

public record UpdateTestCommand(
    Guid Id,
    string Title
) : ICommand<Result<TestResponse>>; 