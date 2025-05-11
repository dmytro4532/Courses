using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Common.Models;
using Courses.Application.Questions.Dto;
using Courses.Domain.Questions;
using Shared.Results;

namespace Courses.Application.Questions.Queries.GetQuestions;

internal sealed class GetQuestionsByTestQueryHandler : IQueryHandler<GetQuestionsByTestQuery, Result<PagedList<QuestionResponse>>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly Mapper<Question, QuestionResponse> _mapper;

    public GetQuestionsByTestQueryHandler(IQuestionRepository questionRepository, Mapper<Question, QuestionResponse> mapper)
    {
        _questionRepository = questionRepository;
        _mapper = mapper;
    }

    public async Task<Result<PagedList<QuestionResponse>>> Handle(GetQuestionsByTestQuery request, CancellationToken cancellationToken)
    {
        var questions = _mapper.Map(
            await _questionRepository.GetByTestIdAsync(
                request.TestId,
                request.PageIndex,
                request.PageSize,
                cancellationToken));

        var totalCount = await _questionRepository.CountByTestIdAsync(request.TestId, cancellationToken);

        return new PagedList<QuestionResponse>(questions, request.PageIndex, request.PageSize, totalCount);
    }
} 