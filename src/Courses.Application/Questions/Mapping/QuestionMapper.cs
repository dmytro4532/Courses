using Courses.Application.Abstractions.Mapping;
using Courses.Application.Questions.Dto;
using Courses.Domain.Questions;

namespace Courses.Application.Questions.Mapping;

internal sealed class QuestionMapper : Mapper<Question, QuestionResponse>
{
    private readonly Mapper<Answer, AnswerResponse> _answerMapper;

    public QuestionMapper(Mapper<Answer, AnswerResponse> answerMapper)
    {
        _answerMapper = answerMapper;
    }

    public override QuestionResponse Map(Question entity)
    {
        return new QuestionResponse(
            entity.Id,
            entity.Content.Value,
            entity.Order.Value,
            entity.Image,
            entity.TestId,
            _answerMapper.Map(entity.Answers));
    }
} 