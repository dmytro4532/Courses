using Courses.Domain.Common;
using Courses.Domain.Courses;
using Courses.Domain.Users;

namespace Courses.Domain.CourseProgresses;

public class CourseProgress : AggregateRoot
{
    public Guid CourseId { get; private set; }

    public Course Course { get; private set; }

    public Guid UserId { get; private set; }

    public User User { get; private set; }

    public bool Completed { get; private set; }

    public DateTime? CompletedAt { get; private set; }

    public DateTime CreatedAt { get; private set; }

    private CourseProgress() { }

    private CourseProgress(Guid id, Guid courseId, Guid userId, bool completed)
        : base(id)
    {
        CourseId = courseId;
        UserId = userId;
        Completed = completed;
        CreatedAt = DateTime.UtcNow;
    }

    public void MarkCompleted()
    {
        Completed = true;
        CompletedAt = DateTime.UtcNow;
    }

    public static CourseProgress Create(Guid id, Guid courseId, Guid userId, bool completed = false)
    {
        return new CourseProgress(id, courseId, userId, completed);
    }
}
