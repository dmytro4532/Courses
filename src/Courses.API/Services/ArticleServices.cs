using MediatR;

namespace Courses.API.Services;

public class ArticleServices(ISender sender)
{
    public ISender Sender { get; set; } = sender;
}
