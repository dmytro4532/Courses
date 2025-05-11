using Courses.Application.Abstractions.Messaging;
using Shared.Results;

namespace Courses.Application.Questions.Commands.DeleteQuestion;

public record DeleteQuestionCommand(Guid Id) : ICommand<Result>; 