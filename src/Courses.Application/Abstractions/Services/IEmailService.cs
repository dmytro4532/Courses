using Courses.Domain.User;

namespace Courses.Application.Abstractions.Services;

public interface IEmailService
{
    public Task SendConfirmaitionEmailAsync(Guid userId, string email);
}
