using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Topics.Dto;
using Courses.Domain.Topics;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Topics.Commands.UpdateTopicMedia;

internal sealed class UpdateTopicMediaCommandHandler : IRequestHandler<UpdateTopicMediaCommand, Result<TopicResponse>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Topic, TopicResponse> _mapper;

    public UpdateTopicMediaCommandHandler(
        ITopicRepository topicRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork,
        Mapper<Topic, TopicResponse> mapper)
    {
        _topicRepository = topicRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TopicResponse>> Handle(UpdateTopicMediaCommand request, CancellationToken cancellationToken)
    {
        var topic = await _topicRepository.GetByIdAsync(request.Id, cancellationToken);

        if (topic is null)
        {
            return new NotFoundError("Topic.NotFound", "The topic was not found.");
        }

        var media = await _fileStorageService.SaveFileAsync(
            request.Media.OpenReadStream(),
            request.Media.ContentType);

        topic.UpdateMedia(media);

        _topicRepository.Update(topic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map(topic);
    }
} 