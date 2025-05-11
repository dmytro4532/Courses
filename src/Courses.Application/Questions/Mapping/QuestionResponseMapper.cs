using Courses.Application.Abstractions.Mapping;
using Courses.Application.Questions.Dto;
using Courses.Domain.Questions;

namespace Courses.Application.Questions.Mapping;

public sealed class QuestionResponseMapper : Mapper<Question, QuestionResponse>
{
    public override QuestionResponse Map(Question source)
    {
        return new QuestionResponse(
            source.Id,
            source.Content.Value,
            source.Order.Value,
            source.Image,
            source.TestId,
            source.CreatedAt,
            source.UpdatedAt);
    }
} 