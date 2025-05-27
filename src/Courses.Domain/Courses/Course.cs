using Courses.Domain.Common;
using Courses.Domain.Courses.DomainEvents;
using Courses.Domain.Topics;

namespace Courses.Domain.Courses;

public class Course : AggregateRoot
{
    private readonly HashSet<Topic> _topics = [];
    
    public Title Title { get; private set; }

    public Description Description { get; private set; }

    public string? Image { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime? UpdatedAt { get; private set; }

    public DateTime? DeletedAt { get; private set; }

    public bool Deleted { get; private set; }

    public IReadOnlyCollection<Topic> Topics => _topics;

    private Course() { }

    private Course(Guid id, Title title, Description description, string? image)
        : base(id)
    {
        Title = title;
        Description = description;
        Image = image;
        CreatedAt = DateTime.UtcNow;
    }

    public void Update(Title title, Description description)
    {
        Title = title;
        Description = description;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseUpdatedDomainEvent(Guid.NewGuid(), Id));
    }

    public void UpdateImage(string? image)
    {
        Image = image;
        UpdatedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseUpdatedDomainEvent(Guid.NewGuid(), Id));
    }

    public void Delete()
    {
        Deleted = true;
        DeletedAt = DateTime.UtcNow;

        AddDomainEvent(new CourseDeletedDomainEvent(Guid.NewGuid(), Id));
    }

    public void AddTopic(Topic topic)
    {
        _topics.Add(topic);
        AddDomainEvent(new CourseUpdatedDomainEvent(Guid.NewGuid(), Id));
    }

    public void RemoveTopic(Topic topic)
    {
        _topics.Remove(topic);
        AddDomainEvent(new CourseUpdatedDomainEvent(Guid.NewGuid(), Id));
    }

    public static Course Create(Guid id, Title title, Description description, string? image)
    {
        var course = new Course(id, title, description, image);

        course.AddDomainEvent(new CourseCreatedDomainEvent(Guid.NewGuid(), course.Id));

        return course;
    }
}
