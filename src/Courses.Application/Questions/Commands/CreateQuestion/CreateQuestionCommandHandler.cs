using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Questions.Dto;
using Courses.Domain.Questions;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Questions.Commands.CreateQuestion;

internal sealed class CreateQuestionCommandHandler : IRequestHandler<CreateQuestionCommand, Result<QuestionResponse>>
{
    private readonly IQuestionRepository _questionRepository;
    private readonly ITestRepository _testRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Question, QuestionResponse> _mapper;

    public CreateQuestionCommandHandler(
        IQuestionRepository questionRepository,
        ITestRepository testRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork,
        Mapper<Question, QuestionResponse> mapper)
    {
        _questionRepository = questionRepository;
        _testRepository = testRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<QuestionResponse>> Handle(CreateQuestionCommand request, CancellationToken cancellationToken)
    {
        var test = await _testRepository.GetByIdAsync(request.TestId, cancellationToken);

        if (test is null)
        {
            return Result.Failure<QuestionResponse>(new NotFoundError("Test.NotFound", "Test not found"));
        }

        string? imageFileName = null;
        if (request.Image != null)
        {
            imageFileName = await _fileStorageService.SaveFileAsync(
                request.Image.OpenReadStream(),
                request.Image.ContentType);
        }

        var question = Question.Create(
            Guid.NewGuid(),
            Content.Create(request.Content),
            Order.Create(request.Order),
            request.TestId);

        question.UpdateImage(imageFileName);

        foreach (var answer in request.Answers)
        {
            question.AddAnswer(Guid.NewGuid(), answer.Value, answer.IsCorrect);
        }

        await _questionRepository.AddAsync(question, cancellationToken);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success(_mapper.Map(question));
    }
} 