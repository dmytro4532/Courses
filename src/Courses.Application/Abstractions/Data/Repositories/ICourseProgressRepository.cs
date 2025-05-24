using Courses.Domain.CourseProgresses;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface ICourseProgressRepository
{
    Task<CourseProgress?> GetByIdAsync(Guid id, CancellationToken cancellationToken);

    Task<CourseProgress?> GetByUserIdAndCourseIdAsync(Guid userId, Guid courseId, CancellationToken cancellationToken);


    Task<IEnumerable<CourseProgress>> GetByUserIdAsync(Guid userId, int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken);

    Task<int> CountByUserIdAsync(Guid userId, CancellationToken cancellationToken);

    Task AddAsync(CourseProgress courseProgress, CancellationToken cancellationToken);

    void Update(CourseProgress courseProgress);

    void Remove(CourseProgress courseProgress);
}
