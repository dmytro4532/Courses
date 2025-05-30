﻿using Courses.Domain.Courses;

namespace Courses.Application.Abstractions.Data.Repositories;

public interface ICourseRepository
{
    Task<IEnumerable<Course>> Get(int pageIndex, int pageSize, string orderBy, string orderDirection, CancellationToken cancellationToken);

    Task<IEnumerable<Course>> GetCoursesByIdsAsync(IEnumerable<Guid> ids, CancellationToken cancellationToken);

    Task<Course?> GetByIdAsync(Guid courseId, CancellationToken cancellationToken);

    Task<int> CountAsync(CancellationToken cancellationToken);

    Task AddAsync(Course course, CancellationToken cancellationToken);

    void Update(Course course);

    void Remove(Course course);
}
