using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Questions.Dto;
using Shared.Results;

namespace Courses.Application.Questions.Queries.GetQuestions;

public record GetQuestionsByTestQuery(
    Guid TestId,
    int PageIndex = 0,
    int PageSize = 10
) : IQuery<Result<PagedList<QuestionResponse>>>
{
    public int PageSize { get; init; } = Math.Min(Math.Max(PageSize, 0), 100);
} 