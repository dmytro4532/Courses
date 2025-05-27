using MediatR;

namespace Courses.API.Services;

public sealed class TopicServices
{
    public required ISender Sender { get; init; }
} 