using MediatR;

namespace Courses.API.Services;

public sealed class TestServices
{
    public required ISender Sender { get; init; }
} 