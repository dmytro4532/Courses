using Courses.Application.Questions.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Questions.Commands.CreateQuestion;

public class CreateQuestionCommand : IRequest<Result<QuestionResponse>>
{
    public required string Content { get; init; } = null!;
    public IFormFile? Image { get; init; }
    public required int Order { get; init; }
    public required Guid TestId { get; init; }
}
