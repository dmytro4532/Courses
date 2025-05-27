using Courses.Application.Abstractions.Messaging;
using Shared.Results;

namespace Courses.Application.Tests.Commands.DeleteTest;

public record DeleteTestCommand(Guid Id) : ICommand<Result>; 