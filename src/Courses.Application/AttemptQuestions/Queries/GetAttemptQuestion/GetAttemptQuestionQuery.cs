using Courses.Application.AttemptQuestions.Dto;
using MediatR;
using Shared.Results;

namespace Courses.Application.AttemptQuestions.Queries.GetAttemptQuestion;

public record GetAttemptQuestionQuery(Guid Id) : IRequest<Result<AttemptQuestionResponse>>; 