using MediatR;

namespace Courses.API.Services;

public sealed class QuestionServices
{
    public required ISender Sender { get; init; }
} 