using Courses.Application.Abstractions.Messaging;
using Courses.Application.Questions.Dto;
using Shared.Results;

namespace Courses.Application.Questions.Commands.UpdateQuestion;

public record UpdateQuestionCommand(
    Guid Id,
    string Content,
    int Order,
    string? Image
) : ICommand<Result<QuestionResponse>>; 