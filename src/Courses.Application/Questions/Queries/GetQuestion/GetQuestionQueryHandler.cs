using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Questions.Dto;
using Courses.Domain.Questions;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Questions.Queries.GetQuestion;

internal sealed class GetQuestionQueryHandler : IQueryHandler<GetQuestionQuery, Result<QuestionResponse>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly Mapper<Question, QuestionResponse> _mapper;

    public GetQuestionQueryHandler(IQuestionRepository questionRepository, Mapper<Question, QuestionResponse> mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    public async Task<Result<QuestionResponse>> Handle(GetQuestionQuery request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (question is null)
        {
            return new NotFoundError("Question.NotFound", "The question was not found.");
        }

        return _mapper.Map(question);
    }
} 