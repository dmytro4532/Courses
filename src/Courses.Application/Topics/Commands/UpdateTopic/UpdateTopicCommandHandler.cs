using Courses.Application.Abstractions.Data;
using Courses.Application.Abstractions.Data.Repositories;
using Courses.Application.Abstractions.Mapping;
using Courses.Application.Topics.Dto;
using Courses.Domain.Topics;
using MediatR;
using Shared.Results;
using Shared.Results.Errors;

namespace Courses.Application.Topics.Commands.UpdateTopic;

internal sealed class UpdateTopicCommandHandler : IRequestHandler<UpdateTopicCommand, Result<TopicResponse>>
{
    private readonly ITopicRepository _topicRepository;
    private readonly ITestRepository _testRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly Mapper<Topic, TopicResponse> _mapper;

    public UpdateTopicCommandHandler(
        ITopicRepository topicRepository,
        ITestRepository testRepository,
        IUnitOfWork unitOfWork,
        Mapper<Topic, TopicResponse> mapper)
    {
        _topicRepository = topicRepository;
        _testRepository = testRepository;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public async Task<Result<TopicResponse>> Handle(UpdateTopicCommand request, CancellationToken cancellationToken)
    {
        var topic = await _topicRepository.GetByIdAsync(request.Id, cancellationToken);

        if (topic is null)
        {
            return new NotFoundError("Topic.NotFound", "The topic was not found.");
        }

        topic.Update(
            Title.Create(request.Title),
            Content.Create(request.Content));

        topic.UpdateOrder(request.Order);

        if (request.TestId != topic.TestId)
        {
            if (request.TestId.HasValue)
            {
                var test = await _testRepository.GetByIdAsync(request.TestId.Value, cancellationToken);
                if (test is null)
                {
                    return new NotFoundError("Test.NotFound", "The test was not found.");
                }
                topic.SetTest(test);
            }
            else
            {
                topic.RemoveTest();
            }
        }

        _topicRepository.Update(topic);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return _mapper.Map(topic);
    }
} 