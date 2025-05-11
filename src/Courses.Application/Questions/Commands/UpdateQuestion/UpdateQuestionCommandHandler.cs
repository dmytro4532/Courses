using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Messaging;
using Courses.Application.Questions.Dto;
using Courses.Domain.Questions;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Questions.Commands.UpdateQuestion;

internal sealed class UpdateQuestionCommandHandler : ICommandHandler<UpdateQuestionCommand, Result<QuestionResponse>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Question, QuestionResponse> _mapper;

    public UpdateQuestionCommandHandler(
        IQuestionRepository questionRepository,
        IUnitOfWork unitOfWork,
        Mapper<Question, QuestionResponse> mapper)
    {
        _questionRepository = questionRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<QuestionResponse>> Handle(UpdateQuestionCommand request, CancellationToken cancellationToken)
    {
        var question = await _questionRepository.GetByIdAsync(request.Id, cancellationToken);

        if (question is null)
        {
            return Result.Failure<QuestionResponse>(new NotFoundError("Question.NotFound", "Question not found"));
        }

        question.Update(
            Content.Create(request.Content),
            Order.Create(request.Order));
        question.UpdateImage(request.Image);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map(question));
    }
} 