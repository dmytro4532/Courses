using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Services;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Topics.Commands.DeleteTopic;

internal sealed class DeleteTopicCommandHandler : IRequestHandler<DeleteTopicCommand, Result>
{
    private readonly ITopicRepository _topicRepository;
    private readonly IFileStorageService _fileStorageService;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteTopicCommandHandler(
        ITopicRepository topicRepository,
        IFileStorageService fileStorageService,
        IUnitOfWork unitOfWork)
    {
        _topicRepository = topicRepository;
        _fileStorageService = fileStorageService;
        _unitOfWork = unitOfWork;
    }

    public async Task<Result> Handle(DeleteTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await _topicRepository.GetByIdAsync(request.TopicId, cancellationToken);

        if (topic is null)
        {
            return new NotFoundError("Topic.NotFound", "The topic was not found.");
        }

        if (!string.IsNullOrEmpty(topic.Media))
        {
            await _fileStorageService.DeleteFileAsync(topic.Media);
        }

        _topicRepository.Remove(topic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
} 