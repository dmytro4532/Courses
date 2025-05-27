using Courses.Domain.Common;
using Courses.Domain.Courses;
using Courses.Domain.Tests;

namespace Courses.Domain.Topics;

public class Topic : AggregateRoot
{
    public Title Title { get; private set; }

    public Content Content { get; private set; }

    public string? Media { get; private set; }

    public int Order { get; private set; }

    public Guid CourseId { get; private set; }

    public Course Course { get; private set; }

    public Guid? TestId { get; private set; }

    public Test? Test { get; private set; }

    private Topic() { }

    private Topic(Guid id, Title title, Content content, string? media, int order, Guid courseId, Guid? testId = null)
        : base(id)
    {
        Title = title;
        Content = content;
        Media = media;
        Order = order;
        CourseId = courseId;
        TestId = testId;
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

    public void SetTest(Test test)
    {
        TestId = test.Id;
        Test = test;
    }

    public void RemoveTest()
    {
        TestId = null;
        Test = null;
    }

    public static Topic Create(Guid id, Title title, Content content, string? media, int order, Guid courseId, Guid? testId = null)
    {
        return new Topic(id, title, content, media, order, courseId, testId);
    }
} 