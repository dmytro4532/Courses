using Courses.Application.Courses.Dto;
using MediatR;
using Microsoft.AspNetCore.Http;
using Shared.Results;

namespace Courses.Application.Courses.Commands.UpdateArticle;

public class UpdateArticleCommand : IRequest<Result<CourseResponse>>
{
    public Guid Id { get; init; }
    public string Title { get; init; } = null!;
    public string Description { get; init; } = null!;
    public IFormFile? Image { get; init; }
}
