using Courses.Domain.Common;
using Courses.Domain.Courses;
using Courses.Domain.Tests;

namespace Courses.Domain.Topics;

public class Topic : AggregateRoot
{
    private readonly HashSet<Test> _tests = [];

    public Title Title { get; private set; }

    public Content Content { get; private set; }

    public string? Media { get; private set; }

    public int Order { get; private set; }

    public Guid CourseId { get; private set; }

    public Course Course { get; private set; }

    public IReadOnlyCollection<Test> Tests => _tests;

    private Topic() { }

    private Topic(Guid id, Title title, Content content, string? media, int order, Guid courseId)
        : base(id)
    {
        Title = title;
        Content = content;
        Media = media;
        Order = order;
        CourseId = courseId;
    }

    public void Update(Title title, Content content)
    {
        Title = title;
        Content = content;
    }

    public void UpdateMedia(string? media)
    {
        Media = media;
    }

    public void UpdateOrder(int order)
    {
        Order = order;
    }

    public void AddTest(Test test)
    {
        _tests.Add(test);
    }

    public void RemoveTest(Test test)
    {
        _tests.Remove(test);
    }

    public static Topic Create(Guid id, Title title, Content content, string? media, int order, Guid courseId)
    {
        return new Topic(id, title, content, media, order, courseId);
    }
} 