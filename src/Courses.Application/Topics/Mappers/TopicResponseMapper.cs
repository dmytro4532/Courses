using Courses.Application.Abstractions.Mapping;
using Courses.Application.Abstractions.Services;
using Courses.Application.Topics.Dto;
using Courses.Domain.Topics;

namespace Courses.Application.Topics.Mappers;

internal sealed class TopicResponseMapper : Mapper<Topic, TopicResponse>
{
    private readonly IFileStorageService _fileStorageService;

    public TopicResponseMapper(IFileStorageService fileStorageService)
    {
        _fileStorageService = fileStorageService;
    }

    public override TopicResponse Map(Topic source)
    {
        return new TopicResponse(
            source.Id,
            source.Title,
            source.Content,
            source.Media is null ? null : _fileStorageService.CreateUrl(source.Media),
            source.Order,
            source.CourseId
        );
    }
} 