using MediatR;

namespace Courses.API.Services;

public class UserServices(ISender sender)
{
    public ISender Sender { get; set; } = sender;
}
