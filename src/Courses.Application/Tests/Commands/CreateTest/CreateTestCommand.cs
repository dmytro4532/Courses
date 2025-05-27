using Courses.Application.Abstractions.Messaging;
using Courses.Application.Tests.Dto;
using Shared.Results;

namespace Courses.Application.Tests.Commands.CreateTest;

public record CreateTestCommand(
    string Title
) : ICommand<Result<TestResponse>>; 