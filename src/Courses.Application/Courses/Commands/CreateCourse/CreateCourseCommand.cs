using Courses.Application.Articles.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.Articles.Commands.CreateArticle;

public record CreateCourseCommand(string Title, string Description) : IRequest<Result<CourseResponse>>;
