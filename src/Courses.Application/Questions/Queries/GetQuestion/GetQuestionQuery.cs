using Courses.Application.Abstractions.Messaging;
using Courses.Application.Questions.Dto;
using Shared.Results;

namespace Courses.Application.Questions.Queries.GetQuestion;

public record GetQuestionQuery(Guid Id) : IQuery<Result<QuestionResponse>>; 