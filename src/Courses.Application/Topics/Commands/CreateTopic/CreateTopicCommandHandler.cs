using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Common.Settings;
using Courses.Application.Topics.Dto;
using Courses.Domain.Topics;
using MediatR;
using Microsoft.Extensions.Options;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Topics.Commands.CreateTopic;

internal sealed class CreateTopicCommandHandler : IRequestHandler<CreateTopicCommand, Result<TopicResponse>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ICourseRepository _courseRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly FileSettings _fileSettings;
    private readonly Mapper<Topic, TopicResponse> _mapper;

    public CreateTopicCommandHandler(
        ITopicRepository topicRepository,
        ICourseRepository courseRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork,
        IOptions<FileSettings> fileSettings,
        Mapper<Topic, TopicResponse> mapper)
    {
        _topicRepository = topicRepository;
        _courseRepository = courseRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
        _fileSettings = fileSettings.Value;
        _mapper = mapper;
    }

    public async Task<Result<TopicResponse>> Handle(CreateTopicCommand request, CancellationToken cancellationToken)
    {
        var course = await _courseRepository.GetByIdAsync(request.CourseId, cancellationToken);

        if (course is null)
        {
            return new NotFoundError("Course.NotFound", "The course was not found.");
        }

        string? media = null;

        if (request.Media is not null)
            media = await _fileStorageService.SaveFileAsync(
                request.Media.OpenReadStream(),
                request.Media.ContentType);

        var topic = Topic.Create(
            Guid.NewGuid(),
            Title.Create(request.Title),
            Content.Create(request.Content),
            media,
            request.Order,
            request.CourseId);

        await _topicRepository.AddAsync(topic, cancellationToken);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map(topic);
    }
} 