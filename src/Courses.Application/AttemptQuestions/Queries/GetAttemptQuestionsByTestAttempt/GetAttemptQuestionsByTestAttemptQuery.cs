using Courses.Application.AttemptQuestions.Dto;
using Courses.Application.Common.Models;
using MediatR;
using Shared.Results;

namespace Courses.Application.AttemptQuestions.Queries.GetAttemptQuestionsByTestAttempt;

public record GetAttemptQuestionsByTestAttemptQuery(
    Guid TestAttemptId,
    int PageIndex,
    int PageSize) : IRequest<Result<PagedList<AttemptQuestionResponse>>>; 