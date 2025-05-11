using Courses.Application.Abstractions.Messaging;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Questions.Commands.UpdateImage;

public class UpdateImageCommand : ICommand<Result>
{
    public required Guid QuestionId { get; init; }
    public IFormFile? Image { get; init; }
}
