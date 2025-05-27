using Courses.Application.Abstractions.Mapping;
using Courses.Application.Questions.Dto;
using Courses.Domain.Questions;

namespace Courses.Application.Questions.Mapping;

internal sealed class AnswerMapper : Mapper<Answer, AnswerResponse>
{
    public override AnswerResponse Map(Answer entity)
    {
        return new AnswerResponse(
            entity.Id,
            entity.Value,
            entity.IsCorrect);
    }
} 