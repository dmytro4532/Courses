using Courses.Domain.Articles.DomainEvents;
using Courses.Domain.Common;

namespace Courses.Domain.Articles;

public class Course : AggregateRoot
{
    public Title Title { get; private set; }

    public Description Description { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool Deleted { get; private set; }

    private Course() { }

    private Course(Guid id, Title title, Description description)
        : base(id)
    {
        Title = title;
        Description = description;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(Title title, Description description)
    {
        Title = title;
        Description = description;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseUpdatedDomainEvent(Guid.NewGuid(), Id));
    }

    public void Delete()
    {
        Deleted = true;
        DeletedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseDeletedDomainEvent(Guid.NewGuid(), Id));
    }

    public static Course Create(Guid id, Title title, Description description)
    {
        var article = new Course(id, title, description);

        article.AddDomainEvent(new CourseCreatedDomainEvent(Guid.NewGuid(), article.Id));

        return article;
    }
}
